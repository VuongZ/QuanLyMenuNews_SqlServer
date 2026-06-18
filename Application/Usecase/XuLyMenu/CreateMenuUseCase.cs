using Domain.entity;
using Domain.repositories;
using Application.Requests.XuLyMenu;
using MediatR;
using FluentValidation;

namespace Application.XuLyMenu.UseCases
{
    public class CreateMenuUseCase : IRequestHandler<CreateMenuRequest,bool>
    {
        private readonly IMenuRepo _menuRepo;
         private readonly INewsRepo _newsRepo;
        private readonly IUnitOfWork _uow;

        public CreateMenuUseCase(IMenuRepo menuRepo, INewsRepo newsRepo, IUnitOfWork uow)
        {
            _menuRepo = menuRepo;
            _newsRepo = newsRepo;
            _uow = uow;
        }


        public async Task<bool> Handle(CreateMenuRequest request, CancellationToken cancellationToken)
        {
            await _uow.BeginTransactionAsync(cancellationToken);
            try
            {
               var existing = await _menuRepo.GetBySlugAsync(request.Slug.Trim().ToLower());
            if (existing != null)
            {
                   throw new ValidationException("Menu đã tồn tại.");
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
                  $"Slug news '{item.Slug}' đã tồn tại với title khác."
            );
                    }
                    menu.News.Add(news);
                     continue;
                    }
               else
                {
                     news = new News
                     {
                    Title = item.Title,
                    Slug = item.Slug.Trim().ToLower(),
                    Content = item.Content,
                     thumbnail = item.Thumbnail,
                    created_at = DateTime.UtcNow,
                    updated_at = DateTime.UtcNow
                     };
                await _newsRepo.AddAsync(news);
                }
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