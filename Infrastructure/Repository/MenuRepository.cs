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
        return await _dbSet.FirstOrDefaultAsync(x => x.Slug == slug && x.is_deleted == false);
    }
     public async Task<Menu?> GetByIdWithNewsAsync(int id)
    {
        return await _context.Menus
            .Where(m => m.Id == id && m.is_deleted == false)
            .Select(m => new Menu
            {
                Id         = m.Id,
                Name       = m.Name,
                Slug       = m.Slug,
                created_at = m.created_at,
                updated_at = m.updated_at,
                is_deleted = m.is_deleted,
                News = m.News
                    .Where(n => n.is_deleted == false)
                    .Select(n => new News
                    {
                        Id        = n.Id,
                        Title     = n.Title,
                        Slug      = n.Slug,
                        Content   = n.Content,
                        thumbnail = n.thumbnail,
                        Address=n.Address,
                        WardId=n.WardId,
                        Ward = n.Ward == null
                        ? null
                        : new WebsiteLocalizationWard
                        {
                            WardId = n.Ward.WardId,
                            WardPid = n.Ward.WardPid,
                            Name = n.Ward.Name,
                            NameEn = n.Ward.NameEn,
                            FullName = n.Ward.FullName,
                            FullNameEn = n.Ward.FullNameEn,
                            KeyLocalization = n.Ward.KeyLocalization,
                            IsActived = n.Ward.IsActived
                        },
                        created_at = n.created_at,
                        updated_at = n.updated_at,
                        deleted_at = n.deleted_at,
                        is_deleted = n.is_deleted
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Menu>> GetAllWithNewsAsync()
    {
        return await _context.Menus
            .Where(m => m.is_deleted == false)
            .Select(m => new Menu
            {
                Id         = m.Id,
                Name       = m.Name,
                Slug       = m.Slug,
                created_at = m.created_at,
                updated_at = m.updated_at,
                is_deleted = m.is_deleted,
               News = m.News
                    .Where(n => n.is_deleted == false)
                    .Select(n => new News
                    {
                        Id        = n.Id,
                        Title     = n.Title,
                        Slug      = n.Slug,
                        Content   = n.Content,
                        thumbnail = n.thumbnail,
                        Address=n.Address,
                        WardId=n.WardId,
                        Ward = n.Ward == null
                        ? null
                        : new WebsiteLocalizationWard
                        {
                            WardId = n.Ward.WardId,
                            WardPid = n.Ward.WardPid,
                            Name = n.Ward.Name,
                            NameEn = n.Ward.NameEn,
                            FullName = n.Ward.FullName,
                            FullNameEn = n.Ward.FullNameEn,
                            KeyLocalization = n.Ward.KeyLocalization,
                            IsActived = n.Ward.IsActived
                        },
                        created_at = n.created_at,
                        updated_at = n.updated_at,
                        deleted_at = n.deleted_at,
                        is_deleted = n.is_deleted
                    })
                    .ToList()
            })
            .ToListAsync();
    }
}