using Domain.entity;
using Domain.repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;
public class NewsRepository : Repository<News> , INewsRepo
{
    public NewsRepository(AppDbContext context) : base(context)
    {
        
    }

    public Task<News?> GetBySlugAsync(string slug)
    {
        return _context.News.FirstOrDefaultAsync(x => x.Slug == slug && x.is_deleted == false);
    }
    public async Task<News?> GetByIdWithMenusAsync(int id)
{
    return await _context.News
        .Where(n => n.Id == id && n.is_deleted == false)
        .Select(n => new News
        {
            Id = n.Id,
            Title = n.Title,
            Slug = n.Slug,
            Content = n.Content,
            thumbnail = n.thumbnail,
            created_at = n.created_at,
                    updated_at = n.updated_at,
                    deleted_at = n.deleted_at,
                    is_deleted = n.is_deleted,
            Menu = n.Menu.Where(m => m.is_deleted == false)
                .Select(m => new Menu
                {
                    Id = m.Id,
                    Name = m.Name,
                    Slug = m.Slug,   created_at = m.created_at,
                    updated_at = m.updated_at,
                    deleted_at = m.deleted_at,
                    is_deleted = m.is_deleted
            }).ToList()
        })
        .FirstOrDefaultAsync();
}
public async Task<IEnumerable<News>> GetAllWithMenusAsync(int page, int pageSize)
    {
        return await _context.News
            .Where(n => n.is_deleted == false)
             .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(n => new News
            {
                Id = n.Id,
                Title = n.Title,
                Slug = n.Slug,
                Content = n.Content,
                thumbnail = n.thumbnail,
                   created_at = n.created_at,
                    updated_at = n.updated_at,
                    deleted_at = n.deleted_at,
                    is_deleted = n.is_deleted,
                Menu = n.Menu.Where(m => m.is_deleted == false)
                .Select(m => new Menu
                {
                    Id = m.Id,
                    Name = m.Name,
                    Slug = m.Slug,  
                     created_at = m.created_at,
                    updated_at = m.updated_at,
                    deleted_at = m.deleted_at,
                    is_deleted = m.is_deleted
                }).ToList()
            })
            .ToListAsync();
    }
}   
                    