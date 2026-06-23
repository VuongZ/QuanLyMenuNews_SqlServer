using Application.Requests.XuLyMenu;
using Domain.entity;
using Domain.repositories;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Application.Usecase.XuLyMenu;

public class UpdateMenuUseCase
    : IRequestHandler<UpdateMenuRequest, bool>
{
    private readonly IMenuRepo _menuRepo;
    private readonly INewsRepo _newsRepo;
    IMenuNewsRepo _menuNewsRepo;
    IWebsiteLocalizationWardRepo _wardRepo;
    private readonly IUnitOfWork _uow;

    public UpdateMenuUseCase(IMenuRepo menuRepo, INewsRepo newsRepo, IUnitOfWork uow, IMenuNewsRepo menuNewsRepo, IWebsiteLocalizationWardRepo wardRepo)
    {
        _menuRepo = menuRepo;
        _newsRepo = newsRepo;
        _uow = uow;
        _menuNewsRepo = menuNewsRepo;
        _wardRepo = wardRepo;
    }

    public async Task<bool> Handle(UpdateMenuRequest request, CancellationToken cancellationToken)
    {
        await _uow.BeginTransactionAsync(cancellationToken);
        try
        {
            var normalizedMenuSlug = request.Slug.Trim().ToLowerInvariant();
            var duplicateMenu = await _menuRepo.GetBySlugAsync(normalizedMenuSlug);
            if (duplicateMenu != null &&
                duplicateMenu.Id != request.Id)
            {
                throw new ValidationException(new[]
                {
                    new ValidationFailure(
                        nameof(request.Slug),
                        $"Slug Menu '{request.Slug}' đã tồn tại."
                    )
                });
            }
            var menu = await _menuRepo.GetByIdAsync(request.Id);
            if (menu == null)
            {
                await _uow.RollbackAsync(cancellationToken);
                return false;
            }
            menu.Name = request.Name.Trim();
            menu.Slug = normalizedMenuSlug;
            menu.updated_at = DateTime.UtcNow;
            await _menuRepo.UpdateAsync(menu);

            var requestedNewsIds = new List<int>();
            foreach (var item in request.DanhSachNews)
            {
                var normalizedNewsSlug = item.Slug.Trim().ToLowerInvariant();
                News news;

                if (item.Id.HasValue)
                {
                    news = await _newsRepo.GetByIdAsync(item.Id.Value);
                    if (news == null)
                    {
                        throw new ValidationException(new[]
                        {
                            new ValidationFailure(
                                nameof(item.Id),
                                $"Không tìm thấy News Id = {item.Id.Value}."
                            )
                        });
                    }
                    var duplicateNews = await _newsRepo.GetBySlugAsync(normalizedNewsSlug);
                    if (duplicateNews != null && duplicateNews.Id != news.Id)
                    {
                        throw new ValidationException(new[]
                        {
                            new ValidationFailure(
                                nameof(item.Slug),
                                $"Slug News '{item.Slug}' đã tồn tại."
                            )
                        });
                    }
                    var wardId = await ResolveWardIdAsync(item.ProvinceId, item.WardId, item.Address);
                    news.Title      = item.Title.Trim();
                    news.Slug       = normalizedNewsSlug;
                    news.Content    = item.Content;
                    news.thumbnail  = string.IsNullOrWhiteSpace(item.Thumbnail) ? null : item.Thumbnail.Trim();
                    news.Address    = string.IsNullOrWhiteSpace(item.Address)   ? null : item.Address.Trim();
                    news.WardId     = wardId;
                    news.updated_at = DateTime.UtcNow;
                    await _newsRepo.UpdateAsync(news);
                }
                else
                {
                    var existingNews = await _newsRepo.GetBySlugAsync(normalizedNewsSlug);
                    if (existingNews != null)
                    {
                        if (!string.Equals(
                                existingNews.Title?.Trim(),
                                item.Title.Trim(),
                                StringComparison.OrdinalIgnoreCase))
                        {
                            throw new ValidationException(new[]
                            {
                                new ValidationFailure(
                                    nameof(item.Slug),
                                    $"Slug News '{item.Slug}' đã tồn tại với tiêu đề khác."
                                )
                            });
                        }
                        news = existingNews;
                    }
                    else
                    {
                        var wardId = await ResolveWardIdAsync(item.ProvinceId, item.WardId, item.Address);
                        news = new News
                        {
                            Title      = item.Title.Trim(),
                            Slug       = normalizedNewsSlug,
                            Content    = item.Content,
                            thumbnail  = string.IsNullOrWhiteSpace(item.Thumbnail) ? null : item.Thumbnail.Trim(),
                            Address    = string.IsNullOrWhiteSpace(item.Address)   ? null : item.Address.Trim(),
                            WardId     = wardId,
                            created_at = DateTime.UtcNow,
                            updated_at = DateTime.UtcNow
                        };

                        await _newsRepo.AddAsync(news);
                        await _uow.SaveChangesAsync(cancellationToken);
                    }
                }
                if (!requestedNewsIds.Contains(news.Id))
                {
                    requestedNewsIds.Add(news.Id);
                }
            }
            var currentNewsIds  = await _menuNewsRepo.GetNewsIdsByMenuIdAsync(menu.Id, cancellationToken);
            var newsIdsToRemove = currentNewsIds.Except(requestedNewsIds).ToList();
            var newsIdsToAdd    = requestedNewsIds.Except(currentNewsIds).ToList();
            await _menuNewsRepo.RemoveByMenuAndNewsIdsAsync(menu.Id, newsIdsToRemove, cancellationToken);
            foreach (var newsId in newsIdsToAdd)
            {
                await _menuNewsRepo.AddAsync(new MenuNews
                {
                    MenuId = menu.Id,
                    NewsId = newsId
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
                    "Phải nhập đầy đủ tỉnh/thành phố, phường/xã và địa chỉ.")
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