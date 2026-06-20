using Domain.entity;
using Domain.repositories;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Application.Requests.XuLyNews;

public class UpdateNewsUseCase
    : IRequestHandler<UpdateNewsRequest, bool>
{
    private readonly INewsRepo _newsRepo;
    private readonly IMenuRepo _menuRepo;
    private readonly IMenuNewsRepo _menuNewsRepo;
    private readonly IWebsiteLocalizationWardRepo _wardRepo;
    private readonly IUnitOfWork _uow;

    public UpdateNewsUseCase(
        INewsRepo newsRepo,
        IMenuRepo menuRepo,
        IMenuNewsRepo menuNewsRepo,
        IWebsiteLocalizationWardRepo wardRepo,
        IUnitOfWork uow)
    {
        _newsRepo = newsRepo;
        _menuRepo = menuRepo;
        _menuNewsRepo = menuNewsRepo;
        _wardRepo = wardRepo;
        _uow = uow;
    }

    public async Task<bool> Handle(UpdateNewsRequest request,CancellationToken cancellationToken)
    {
        await _uow.BeginTransactionAsync(cancellationToken);
        try
        {
            var normalizedNewsSlug = request.slug .Trim().ToLowerInvariant();
            var duplicateNews = await _newsRepo.GetBySlugAsync(normalizedNewsSlug);
            if (duplicateNews != null &&
                duplicateNews.Id != request.id)
            {
                throw new ValidationException(new[]
                {
                    new ValidationFailure(
                        nameof(request.slug),
                        $"Slug News '{request.slug}' đã tồn tại."
                    )
                });
            }

            var news = await _newsRepo.GetByIdAsync(request.id);
            if (news == null)
            {
                await _uow.RollbackAsync(cancellationToken);
                return false;
            }
            var wardId = await ResolveWardIdAsync(
                request.ProvinceName,
                request.WardName,
                request.Address);
            news.Title = request.title?.Trim();
            news.Slug = normalizedNewsSlug;
            news.Content = request.content;
            news.thumbnail =
                string.IsNullOrWhiteSpace(request.thumbnail)
                    ? null
                    : request.thumbnail.Trim();

            news.Address =
                string.IsNullOrWhiteSpace(request.Address)
                    ? null
                    : request.Address.Trim();

            news.WardId = wardId;
            news.updated_at = DateTime.UtcNow;
            await _newsRepo.UpdateAsync(news);
            var requestedMenuIds = new List<int>();
            foreach (var item in request.DanhSachMenus)
            {
                var normalizedMenuSlug = item.Slug.Trim().ToLowerInvariant();
                Menu menu;
                if (item.Id.HasValue)
                {
                    menu = await _menuRepo.GetByIdAsync(item.Id.Value);
                    if (menu == null)
                    {
                        throw new ValidationException(new[]
                        {
                            new ValidationFailure(
                                nameof(item.Id),
                                $"Không tìm thấy Menu Id = {item.Id.Value}."
                            )
                        });
                    }
                    var duplicateMenu = await _menuRepo.GetBySlugAsync(normalizedMenuSlug);
                    if (duplicateMenu != null && duplicateMenu.Id != menu.Id)
                    {
                        throw new ValidationException(new[]
                        {
                            new ValidationFailure(
                                nameof(item.Slug),
                                $"Slug Menu '{item.Slug}' đã tồn tại."
                            )
                        });
                    }
                    menu.Name = item.Name.Trim();
                    menu.Slug = normalizedMenuSlug;
                    menu.updated_at = DateTime.UtcNow;
                    await _menuRepo.UpdateAsync(menu);
                }
                else
                {
    
                    var existingMenu = await _menuRepo.GetBySlugAsync(normalizedMenuSlug);
                    if (existingMenu != null)
                    {
                        if (!string.Equals(existingMenu.Name?.Trim(),item.Name.Trim(), StringComparison.OrdinalIgnoreCase))
                        {
                            throw new ValidationException(new[]
                            {
                                new ValidationFailure(
                                    nameof(item.Slug),
                                    $"Slug Menu '{item.Slug}' đã tồn tại với tên khác."
                                )
                            });
                        }
                        menu = existingMenu;
                    }
                    else
                    {
                        menu = new Menu
                        {
                            Name = item.Name.Trim(),
                            Slug = normalizedMenuSlug,
                            created_at = DateTime.UtcNow,
                            updated_at = DateTime.UtcNow
                        };
                        await _menuRepo.AddAsync(menu);
                        await _uow.SaveChangesAsync(cancellationToken);
                    }
                }
                if (!requestedMenuIds.Contains(menu.Id))
                {
                    requestedMenuIds.Add(menu.Id);
                }
            }
            var currentMenuIds = await _menuNewsRepo.GetMenuIdsByNewsIdAsync(news.Id,cancellationToken);
            var menuIdsToRemove = currentMenuIds.Except(requestedMenuIds).ToList();
            var menuIdsToAdd = requestedMenuIds.Except(currentMenuIds).ToList();
            await _menuNewsRepo.RemoveByNewsAndMenuIdsAsync(news.Id,menuIdsToRemove,cancellationToken);
            foreach (var menuId in menuIdsToAdd)
            {
                await _menuNewsRepo.AddAsync(new MenuNews
                {
                    MenuId = menuId,
                    NewsId = news.Id
                });
            }
            await _uow.CommitAsync(cancellationToken);
            return true;
        }
        catch
        {
            await _uow.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private async Task<int?> ResolveWardIdAsync(string? provinceName,string? wardName,string? address)
    {
        var hasProvince =!string.IsNullOrWhiteSpace(provinceName);
        var hasWard =!string.IsNullOrWhiteSpace(wardName);
        var hasAddress =!string.IsNullOrWhiteSpace(address);

        if (!hasProvince &&!hasWard && !hasAddress)
        {
            return null;
        }

        if (!hasProvince ||!hasWard ||!hasAddress)
        {
            throw new ValidationException(new[]
            {
                new ValidationFailure(
                    nameof(address),
                    "Phải nhập đầy đủ tỉnh/thành phố, phường/xã và địa chỉ."
                )
            });
        }
        var province = await _wardRepo
            .GetProvinceByNameAsync(
                provinceName!.Trim());
        if (province == null)
        {
            throw new ValidationException(new[]
            {
                new ValidationFailure(
                    nameof(provinceName),
                    $"Không tìm thấy tỉnh/thành phố '{provinceName}'."
                )
            });
        }
        var ward = await _wardRepo
            .GetWardByNameAndProvinceAsync(
                wardName!.Trim(),
                province.WardId);
        if (ward == null)
        {
            throw new ValidationException(new[]
            {
                new ValidationFailure(
                    nameof(wardName),
                    $"Không tìm thấy phường/xã '{wardName}' thuộc '{province.FullName}'."
                )
            });
        }
        return ward.WardId;
    }
}