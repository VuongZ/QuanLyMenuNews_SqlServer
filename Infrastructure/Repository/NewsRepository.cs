using Application.DTO;
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
        return context.News.FirstOrDefaultAsync(x => x.Slug == slug);
    }
    public IQueryable<News> GetByIdWithMenusAsync(int id)
    {
        return  context.News.Where(m => m.Id == id && !m.IsDeleted);
    }

    public IQueryable<News> GetAllWithMenusAsync()
    {
        return context.News.Where(n => !n.IsDeleted).AsSplitQuery();
    }
}   
                    