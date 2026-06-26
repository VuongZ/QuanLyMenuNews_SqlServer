
using Domain.entity;
using Domain.repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;
public class MenuRepository : Repository<Menu> , IMenuRepo
{
    public MenuRepository(AppDbContext context) : base(context)
    {}
          public async Task<Menu?> GetBySlugAsync(string slug)
    {
        return await dbset.FirstOrDefaultAsync(x => x.Slug == slug && x.IsDeleted == false);
    }
     public  IQueryable<Menu> GetByIdWithNewsAsync(int id)
    {
        return context.Menus .Where(m => m.Id == id && !m.IsDeleted);
        
            
    }
    public IQueryable<Menu> GetAllWithNewsAsync()
    {
        return context.Menus.Where(n => !n.IsDeleted).AsSplitQuery();
    }


}