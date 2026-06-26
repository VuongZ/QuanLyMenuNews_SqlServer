using Domain.entity;
using Domain.repositories;
using Application.Requests.XuLyMenu;
using MediatR;
using FluentValidation;
using FluentValidation.Results;

namespace Application.XuLyMenu.UseCases
{
    public class CreateMenuUseCase : IRequestHandler<CreateMenuRequest, bool>
    {
        private readonly IMenuRepo menuRepo;
        private readonly INewsRepo newsRepo;
        private readonly IUnitOfWork _uow;
        private readonly IWebsiteLocalizationWardRepo _wardRepo;

        public CreateMenuUseCase(IMenuRepo menuRepo,INewsRepo newsRepo,IUnitOfWork uow,IWebsiteLocalizationWardRepo wardRepo)
        {
            menuRepo  = menuRepo;
            newsRepo  = newsRepo;
            _uow       = uow;
            _wardRepo  = wardRepo;
        }
        public async Task<bool> Handle(CreateMenuRequest request, CancellationToken cancellationToken)
        {
            await _uow.BeginTransactionAsync(cancellationToken);
            try
            {
                var existing = await menuRepo.GetBySlugAsync(request.Slug.Trim().ToLower());
                if (existing != null)
                {
                    throw new ValidationException(new[]
                    {
                        new ValidationFailure(
                            nameof(request.Slug),
                            $"Slug menu '{request.Slug}' đã tồn tại.")
                    });
                }
                var menu = new Menu
                {
                    Name       = request.Name,
                    Slug       = request.Slug.Trim().ToLower(),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                foreach (var item in request.DanhSachNews)
                {
                    var news = await newsRepo.GetBySlugAsync(item.Slug.Trim().ToLower());
                    if (news != null)
                    {
                        if (!string.Equals(news.Title?.Trim(), item.Title.Trim(), StringComparison.OrdinalIgnoreCase))
                        {
                            throw new ValidationException(
                                $"Slug news '{item.Slug}' đã tồn tại với tiêu đề khác.");
                        }
                        menu.News.Add(news);
                        continue;
                    }
                    var wardId = await ResolveWardIdAsync(item.ProvinceId,item.WardId,item.Address);
                    news = new News
                    {
                        Title      = item.Title,
                        Slug       = item.Slug.Trim().ToLower(),
                        Content    = item.Content,
                        Thumbnail  = item.Thumbnail,
                        Address    = string.IsNullOrWhiteSpace(item.Address) ? null : item.Address.Trim(),
                        WardId     = wardId,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    await newsRepo.AddAsync(news);
                    menu.News.Add(news);
                }
                await menuRepo.AddAsync(menu);
                await _uow.CommitAsync(cancellationToken);
                return true;
            }
            catch
            {
                await _uow.RollbackAsync(cancellationToken);
                throw;
            }
        }
        private async Task<int?> ResolveWardIdAsync(int? provinceId, int? wardId, string? address)
        {
            var hasProvince = provinceId.HasValue;
            var hasWard     = wardId.HasValue;
            var hasAddress  = !string.IsNullOrWhiteSpace(address);
            if (!hasProvince && !hasWard && !hasAddress)
                return null;
            if (!hasProvince || !hasWard || !hasAddress)
            {
                throw new ValidationException(new[]
                {
                    new ValidationFailure(
                        "Address",
                        "Phải nhập đầy đủ tỉnh, phường/xã và địa chỉ.")
                });
            }
            var province = await _wardRepo.GetByIdAsync(provinceId!.Value);
            if (province == null || province.WardPid != 0)
            {
                throw new ValidationException(new[]
                {
                    new ValidationFailure(
                        nameof(provinceId),
                        $"Không tìm thấy tỉnh/thành phố có Id = {provinceId}.")
                });
            }
            var ward = await _wardRepo.GetByIdAsync(wardId!.Value);
            if (ward == null)
            {
                throw new ValidationException(new[]
                {
                    new ValidationFailure(
                        nameof(wardId),
                        $"Không tìm thấy phường/xã có Id = {wardId}.")
                });
            }
            if (ward.WardPid != provinceId!.Value)
            {
                throw new ValidationException(new[]
                {
                    new ValidationFailure(
                        nameof(wardId),
                        $"Phường/xã Id = {wardId} không thuộc tỉnh Id = {provinceId}.")
                });
            }
            return ward.WardId;
        }
    }
}