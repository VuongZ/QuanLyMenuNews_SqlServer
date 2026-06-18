using Domain.entity;
using Domain.repositories;
using Application.Requests.XuLyMenu;
using MediatR;
using Application.Requests.XuLyNews;
using FluentValidation;

namespace Application.XuLyNews.UseCases
{
    public class CreateNewsUseCase : IRequestHandler<CreateNewsRequest,bool>
    {
        private readonly IMenuRepo _menuRepo;
         private readonly INewsRepo _newsRepo;
        private readonly IUnitOfWork _uow;

        public CreateNewsUseCase(IMenuRepo menuRepo, INewsRepo newsRepo, IUnitOfWork uow)
        {
            _menuRepo = menuRepo;
            _newsRepo = newsRepo;
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
                   throw new ValidationException("Tin tức đã tồn tại.");
            }
            var news = new News
            {
                Title = request.Title,
                Slug = request.Slug.Trim().ToLower(),
                Content = request.Content,
                thumbnail = request.Thumbnail,
                created_at = DateTime.UtcNow,
                updated_at = DateTime.UtcNow
            };
            foreach (var item in request.DanhSachMenus)
            {
                var menu = await _menuRepo.GetBySlugAsync(item.Slug.Trim().ToLower());
                if(menu !=null)
                    {
                        if(!string.Equals(menu.Name?.Trim(),item.Name.Trim(),StringComparison.OrdinalIgnoreCase))
                        {
                            throw new ValidationException(
                                $"Slug Menu '{item.Name}' đã tồn tại với Tên khác."
                            );
                        }
                         menu.News.Add(news);
                        continue;

                    }
                else
                {
                     menu = new Menu
                     {
                    Name = item.Name,
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
    }
