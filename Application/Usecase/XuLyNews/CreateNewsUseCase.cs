using Domain.entity;
using Domain.repositories;
using MediatR;
using Application.Requests.XuLyNews;
using FluentValidation;
using FluentValidation.Results;

namespace Application.XuLyNews.UseCases;

public class CreateNewsUseCase
    : IRequestHandler<CreateNewsRequest, bool>
{
    private readonly IMenuRepo _menuRepo;
    private readonly INewsRepo _newsRepo;
    private readonly IWebsiteLocalizationWardRepo _wardRepo;
    private readonly IUnitOfWork _uow;

    public CreateNewsUseCase( IMenuRepo menuRepo,INewsRepo newsRepo,IWebsiteLocalizationWardRepo wardRepo,IUnitOfWork uow)
    {
        _menuRepo = menuRepo;
        _newsRepo = newsRepo;
        _wardRepo = wardRepo;
        _uow = uow;
    }

    public async Task<bool> Handle( CreateNewsRequest request,CancellationToken cancellationToken)
    {
        await _uow.BeginTransactionAsync(cancellationToken);
        try
        {
            var existing = await _newsRepo.GetBySlugAsync(request.Slug.Trim().ToLower());
            if (existing != null)
            {
                throw new ValidationException(new[]
                {
                    new ValidationFailure(
                        nameof(request.Slug),
                        $"Slug news '{request.Slug}' đã tồn tại."
                    )
                });
            }
            WebsiteLocalizationWard? selectedWard = null;
            var hasProvince =!string.IsNullOrWhiteSpace(request.ProvinceName);
            var hasWard =!string.IsNullOrWhiteSpace(request.WardName);
            if (hasProvince || hasWard)
            {
                if (!hasProvince || !hasWard)
                {
                    throw new ValidationException(new[]
                    {
                        new ValidationFailure(
                            "Address",
                            "Phải nhập đầy đủ tỉnh/thành phố và phường/xã."
                        )
                    });
                }

                var province = await _wardRepo.GetProvinceByNameAsync(request.ProvinceName!.Trim());
                if (province == null)
                {
                    throw new ValidationException(new[]
                    {
                        new ValidationFailure(
                            nameof(request.ProvinceName),
                            $"Không tìm thấy tỉnh/thành phố " +
                            $"'{request.ProvinceName}'."
                        )
                    });
                }
                selectedWard = await _wardRepo.GetWardByNameAndProvinceAsync( request.WardName!.Trim(),province.WardId);
                if (selectedWard == null)
                {
                    throw new ValidationException(new[]
                    {
                        new ValidationFailure(nameof(request.WardName),
                            $"Không tìm thấy phường/xã " +
                            $"'{request.WardName}' thuộc " +
                            $"'{province.FullName}'."
                        )
                    });
                }
            }
            var news = new News
            {
                Title = request.Title.Trim(),
                Slug = request.Slug.Trim().ToLower(),
                Content = request.Content,
                thumbnail = request.Thumbnail,
                Address = string.IsNullOrWhiteSpace(request.Address)
                    ? null
                    : request.Address.Trim(),
                WardId = selectedWard?.WardId,
                created_at = DateTime.UtcNow,
                updated_at = DateTime.UtcNow
            };

            foreach (var item in request.DanhSachMenus)
            {
                var menu = await _menuRepo.GetBySlugAsync(item.Slug.Trim().ToLower());
                if (menu != null)
                {
                    if (!string.Equals(menu.Name?.Trim(),item.Name.Trim(),StringComparison.OrdinalIgnoreCase))
                    {
                        throw new ValidationException(new[]
                        {
                            new ValidationFailure(
                                nameof(item.Slug),
                                $"Slug menu '{item.Slug}' " +
                                "đã tồn tại với tên khác."
                            )
                        });
                    }
                }
                else
                {
                    menu = new Menu
                    {
                        Name = item.Name.Trim(),
                        Slug = item.Slug.Trim().ToLower(),
                        created_at = DateTime.UtcNow,
                        updated_at = DateTime.UtcNow
                    };

                    await _menuRepo.AddAsync(menu);
                }
                news.Menu.Add(menu);
            }
            await _newsRepo.AddAsync(news);
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