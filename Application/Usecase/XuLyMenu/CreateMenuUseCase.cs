using Domain.entity;
using Domain.repositories;
using Application.Requests.XuLyMenu;
using MediatR;
using FluentValidation;
using FluentValidation.Results;

namespace Application.XuLyMenu.UseCases
{
    public class CreateMenuUseCase : IRequestHandler<CreateMenuRequest,bool>
    {
        private readonly IMenuRepo _menuRepo;
         private readonly INewsRepo _newsRepo;
        private readonly IUnitOfWork _uow;
        private readonly IWebsiteLocalizationWardRepo _wardRepo;
        public CreateMenuUseCase(IMenuRepo menuRepo, INewsRepo newsRepo, IUnitOfWork uow,IWebsiteLocalizationWardRepo wardRepo)
        {
            _menuRepo = menuRepo;
            _newsRepo = newsRepo;
            _uow = uow;
            _wardRepo=wardRepo;
        }
        public async Task<bool> Handle(CreateMenuRequest request, CancellationToken cancellationToken)
        {
            await _uow.BeginTransactionAsync(cancellationToken);
            try
            {
               var existing = await _menuRepo.GetBySlugAsync(request.Slug.Trim().ToLower());
           if (existing != null)
            {
                throw new ValidationException(new[]
                {
                    new ValidationFailure(
                        nameof(request.Slug),
                        $"Slug menu '{request.Slug}' đã tồn tại."
                    )
                });
            }
            var menu = new Menu
            {
                Name = request.Name,
                Slug = request.Slug.Trim().ToLower(),
                created_at = DateTime.UtcNow,
                updated_at = DateTime.UtcNow
            };
            foreach (var item in request.DanhSachNews)
            {
                var news = await _newsRepo.GetBySlugAsync(item.Slug.Trim().ToLower());
                if(news != null)
                    {
                    if (!string.Equals(
                     news.Title?.Trim(),
                    item.Title.Trim(),
                    StringComparison.OrdinalIgnoreCase))
                    {
                        throw new ValidationException(
                        $"Slug news '{item.Slug}' đã tồn tại với Tiêu Đề khác."
                        );
                    }
                    menu.News.Add(news);
                    continue;
                    }
                    WebsiteLocalizationWard? selectedWard = null;
                    var hasProvince =!string.IsNullOrWhiteSpace(item.ProvinceName);
                    var hasWard =!string.IsNullOrWhiteSpace(item.WardName);
                    var hasAddress = !string.IsNullOrWhiteSpace(item.Address);
                    if (hasProvince || hasWard || hasAddress)
                    {
                        if (!hasProvince || !hasWard || !hasAddress)
                        {
                            throw new ValidationException(new[]
                            {
                                new ValidationFailure(
                                    nameof(item.Address),
                                    $"News '{item.Title}' phải nhập đầy đủ " +
                                    "tỉnh/thành phố, phường/xã và địa chỉ."
                                )
                            });
                        }
                        var province = await _wardRepo.GetProvinceByNameAsync(item.ProvinceName!.Trim());
                    if (province == null)
                    {
                        throw new ValidationException(new[]
                        {
                            new ValidationFailure(
                                nameof(item.ProvinceName),
                                $"Không tìm thấy tỉnh/thành phố " +
                                $"'{item.ProvinceName}'."
                            )
                        });
                    }
                    selectedWard = await _wardRepo.GetWardByNameAndProvinceAsync(item.WardName!.Trim(),province.WardId);
                    if (selectedWard == null)
                    {
                        throw new ValidationException(new[]
                        {
                            new ValidationFailure(
                                nameof(item.WardName),
                                $"Không tìm thấy phường/xã '{item.WardName}' " +
                                $"thuộc '{province.FullName}'."
                            )
                        });
                    }
                }
                    news = new News
                    {
                    Title = item.Title,
                    Slug = item.Slug.Trim().ToLower(),
                    Content = item.Content,
                    thumbnail = item.Thumbnail,
                    Address = string.IsNullOrWhiteSpace(item.Address)? null: item.Address.Trim(),
                    WardId = selectedWard?.WardId,
                    created_at = DateTime.UtcNow,
                    updated_at = DateTime.UtcNow
                    };
                await _newsRepo.AddAsync(news);
                menu.News.Add(news);
                }
            await _menuRepo.AddAsync(menu);
            await _uow.CommitAsync(cancellationToken);
            return true;
            }
            catch
            {
                await _uow.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}