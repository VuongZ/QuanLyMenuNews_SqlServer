using Domain.entity;
using Domain.repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class MenuNewsRepository : IMenuNewsRepo
    {
        private readonly AppDbContext context;

        public MenuNewsRepository(AppDbContext db)
        {
            context = db;
        }

        public IQueryable<MenuNews> GetAllAsync()
        {
            return Query();
        }
        public IQueryable<MenuNews> SearchAsync(int? menuId, int? newsId)
        {
            var query = Query();
            if (menuId.HasValue)
            {
                query = query.Where(x => x.MenuId == menuId.Value);
            }
            if (newsId.HasValue)
            {
                query = query.Where(x => x.NewsId == newsId.Value);
            }
            return query;
        }

        public IQueryable<MenuNews> GetByIdAsync(int menuId, int newsId)
        {
            return Query().Where(x => x.MenuId == menuId && x.NewsId == newsId);
        }

        public async Task AddAsync(MenuNews menuNews)
        {
            await context.MenuNews.AddAsync(menuNews);
        }

        public Task UpdateAsync(MenuNews menuNews)
        {
            context.MenuNews.Update(menuNews);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int menuId, int newsId)
        {
            var menuNews = await context.MenuNews
                .FirstOrDefaultAsync(x => x.MenuId == menuId && x.NewsId == newsId);

            if (menuNews != null)
            {
                context.MenuNews.Remove(menuNews);
            }
        }
        public Task<bool> ExistsAsync(int menuId, int newsId)
        {
            return context.MenuNews.AnyAsync(x =>x.MenuId == menuId && x.NewsId == newsId);
        }

        private IQueryable<MenuNews> Query()
        {
            return context.MenuNews
                .Where(x => x.Menu != null
                    && x.News != null
                    && x.Menu.IsDeleted == false
                    && x.News.IsDeleted == false)
                .Select(x => new MenuNews
                {
                    MenuId = x.MenuId,
                    NewsId = x.NewsId,
                    Menu = new Menu
                    {
                        Id = x.Menu!.Id,
                        Name = x.Menu.Name,
                        Slug = x.Menu.Slug,
                        IsDeleted = x.Menu.IsDeleted,
                        CreatedAt = x.Menu.CreatedAt,
                        UpdatedAt = x.Menu.UpdatedAt,
                        DateledAt = x.Menu.DateledAt
                    },
                    News = new News
                    {
                        Id = x.News!.Id,
                        Title = x.News.Title,
                        Slug = x.News.Slug,
                        Content = x.News.Content,
                        Thumbnail = x.News.Thumbnail,
                        IsDeleted = x.News.IsDeleted,
                        CreatedAt = x.News.CreatedAt,
                        UpdatedAt = x.News.UpdatedAt,
                        DateledAt = x.News.DateledAt
                    }
                });
        }
        public async Task<int[]> GetNewsIdsByMenuIdAsync(int menuId,CancellationToken cancellationToken = default)
            {
                return await context.MenuNews
                    .Where(x => x.MenuId == menuId)
                    .Select(x => x.NewsId)
                    .ToArrayAsync(cancellationToken);
            }

        public async Task RemoveByMenuAndNewsIdsAsync(int menuId,IEnumerable<int> newsIds,CancellationToken cancellationToken = default)
                    {
                        var ids = newsIds.Distinct().ToArray();
                        if (ids.Length == 0){return;}
                        await context.MenuNews
                            .Where(x =>x.MenuId == menuId &&ids.Contains(x.NewsId))
                            .ExecuteDeleteAsync(cancellationToken);
                    }
        public async Task<int[]> GetMenuIdsByNewsIdAsync(int newsId,CancellationToken cancellationToken = default)
                    {
                        return await context.MenuNews
                            .Where(x => x.NewsId == newsId)
                            .Select(x => x.MenuId)
                            .ToArrayAsync(cancellationToken);
                    }
                    public async Task RemoveByNewsAndMenuIdsAsync(int newsId,IEnumerable<int> menuIds,CancellationToken cancellationToken = default)
                    {
                        var ids = menuIds.Distinct().ToArray();
                        if (ids.Length == 0)
                        {
                            return;
                        }
                        await context.MenuNews
                            .Where(x =>x.NewsId == newsId &&ids.Contains(x.MenuId))
                            .ExecuteDeleteAsync(cancellationToken);
                    }
    }
    
    
}
