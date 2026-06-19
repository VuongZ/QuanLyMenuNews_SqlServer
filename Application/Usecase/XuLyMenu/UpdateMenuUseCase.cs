using Application.Requests.XuLyMenu;
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
    private readonly IUnitOfWork _uow;

    public UpdateMenuUseCase(
        IMenuRepo menuRepo,
        INewsRepo newsRepo,
        IUnitOfWork uow)
    {
        _menuRepo = menuRepo;
        _newsRepo = newsRepo;
        _uow = uow;
    }

    public async Task<bool> Handle(
        UpdateMenuRequest request,
        CancellationToken cancellationToken)
    {
        await _uow.BeginTransactionAsync(cancellationToken);

        try
        {
            var normalizedMenuSlug = request.Slug
                .Trim()
                .ToLowerInvariant();

            var duplicateMenu = await _menuRepo
                .GetBySlugAsync(normalizedMenuSlug);

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

            // Lấy Menu thật để cập nhật
            var menu = await _menuRepo
                .GetByIdAsync(request.Id);

            if (menu == null)
            {
                return false;
            }

            menu.Name = request.Name.Trim();
            menu.Slug = normalizedMenuSlug;
            menu.updated_at = DateTime.UtcNow;

            await _menuRepo.UpdateAsync(menu);

            // Cập nhật từng News con
            foreach (var item in request.DanhSachNews)
            {
                if (!item.Id.HasValue)
                {
                    throw new ValidationException(new[]
                    {
                        new ValidationFailure(
                            nameof(item.Id),
                            "News cần cập nhật phải có Id."
                        )
                    });
                }

                var news = await _newsRepo
                    .GetByIdAsync(item.Id.Value);

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

                var normalizedNewsSlug = item.Slug
                    .Trim()
                    .ToLowerInvariant();

                var duplicateNews = await _newsRepo
                    .GetBySlugAsync(normalizedNewsSlug);

                if (duplicateNews != null &&
                    duplicateNews.Id != news.Id)
                {
                    throw new ValidationException(new[]
                    {
                        new ValidationFailure(
                            nameof(item.Slug),
                            $"Slug News '{item.Slug}' đã tồn tại."
                        )
                    });
                }

                news.Title = item.Title.Trim();
                news.Slug = normalizedNewsSlug;
                news.Content = item.Content;
                news.thumbnail = item.Thumbnail;
                news.updated_at = DateTime.UtcNow;

                await _newsRepo.UpdateAsync(news);
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
}