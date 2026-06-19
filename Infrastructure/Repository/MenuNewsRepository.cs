using Domain.entity;
using Domain.repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class MenuNewsRepository : IMenuNewsRepo
    {
        private readonly AppDbContext _context;

        public MenuNewsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MenuNews>> GetAllAsync()
        {
            return await Query()
                .ToListAsync();
        }

        public async Task<IEnumerable<MenuNews>> SearchAsync(int? menuId, int? newsId)
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

            return await query.ToListAsync();
        }

        public async Task<MenuNews?> GetByIdAsync(int menuId, int newsId)
        {
            return await Query()
                .FirstOrDefaultAsync(x => x.MenuId == menuId && x.NewsId == newsId);
        }

        public async Task AddAsync(MenuNews menuNews)
        {
            await _context.MenuNews.AddAsync(menuNews);
        }

        public Task UpdateAsync(MenuNews menuNews)
        {
            _context.MenuNews.Update(menuNews);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int menuId, int newsId)
        {
            var menuNews = await _context.MenuNews
                .FirstOrDefaultAsync(x => x.MenuId == menuId && x.NewsId == newsId);

            if (menuNews != null)
            {
                _context.MenuNews.Remove(menuNews);
            }
        }
        public Task<bool> ExistsAsync(int menuId, int newsId)
{
    return _context.MenuNews.AnyAsync(x =>
        x.MenuId == menuId &&
        x.NewsId == newsId);
}

        private IQueryable<MenuNews> Query()
        {
            return _context.MenuNews
                .Where(x => x.Menu != null
                    && x.News != null
                    && x.Menu.is_deleted == false
                    && x.News.is_deleted == false)
                .Select(x => new MenuNews
                {
                    MenuId = x.MenuId,
                    NewsId = x.NewsId,
                    Menu = new Menu
                    {
                        Id = x.Menu!.Id,
                        Name = x.Menu.Name,
                        Slug = x.Menu.Slug,
                        is_deleted = x.Menu.is_deleted,
                        created_at = x.Menu.created_at,
                        updated_at = x.Menu.updated_at,
                        deleted_at = x.Menu.deleted_at
                    },
                    News = new News
                    {
                        Id = x.News!.Id,
                        Title = x.News.Title,
                        Slug = x.News.Slug,
                        Content = x.News.Content,
                        thumbnail = x.News.thumbnail,
                        is_deleted = x.News.is_deleted,
                        created_at = x.News.created_at,
                        updated_at = x.News.updated_at,
                        deleted_at = x.News.deleted_at
                    }
                });
        }
    }
}
