using Domain.entity;
using Domain.repositories;
using MediatR;
using Application.Requests.XuLyNews;
using FluentValidation;
using FluentValidation.Results;

namespace Application.XuLyNews.UseCases;

public class CreateNewsUseCase : IRequestHandler<CreateNewsRequest, bool>
{
    private readonly IMenuRepo _menuRepo;
    private readonly INewsRepo _newsRepo;
    private readonly IWebsiteLocalizationWardRepo _wardRepo;
    private readonly IUnitOfWork _uow;

    public CreateNewsUseCase(IMenuRepo menuRepo,INewsRepo newsRepo,IWebsiteLocalizationWardRepo wardRepo,IUnitOfWork uow)
    {
        _menuRepo = menuRepo;
        _newsRepo = newsRepo;
        _wardRepo = wardRepo;
        _uow = uow;
    }
    public async Task<bool> Handle(CreateNewsRequest request, CancellationToken cancellationToken)
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
                        $"Slug news '{request.Slug}' đã tồn tại.")
                });
            }
            var wardId = await ResolveWardIdAsync(request.ProvinceId,request.WardId,request.Address);
            var news = new News
            {
                Title      = request.Title.Trim(),
                Slug       = request.Slug.Trim().ToLower(),
                Content    = request.Content,
                thumbnail  = request.Thumbnail,
                Address    = string.IsNullOrWhiteSpace(request.Address) ? null : request.Address.Trim(),
                WardId     = wardId,
                created_at = DateTime.UtcNow,
                updated_at = DateTime.UtcNow
            };
            foreach (var item in request.DanhSachMenus)
            {
                var menu = await _menuRepo.GetBySlugAsync(item.Slug.Trim().ToLower());
                if (menu != null)
                {
                    if (!string.Equals(menu.Name?.Trim(), item.Name.Trim(), StringComparison.OrdinalIgnoreCase))
                    {
                        throw new ValidationException(new[]
                        {
                            new ValidationFailure(
                                nameof(item.Slug),
                                $"Slug menu '{item.Slug}' đã tồn tại với tên khác.")
                        });
                    }
                }
                else
                {
                    menu = new Menu
                    {
                        Name       = item.Name.Trim(),
                        Slug       = item.Slug.Trim().ToLower(),
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