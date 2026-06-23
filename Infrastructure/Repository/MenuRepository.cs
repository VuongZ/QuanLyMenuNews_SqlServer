
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
        return await GetAllWithNewsQuery().FirstOrDefaultAsync(m => m.Id == id);
            
    }

    public IAsyncEnumerable<Menu> GetAllWithNewsAsync()
    {
        return  GetAllWithNewsQuery().AsAsyncEnumerable();
     
    }
    private IQueryable<Menu> GetAllWithNewsQuery()
{
    return _context.Menus
        .Where(m => !m.is_deleted)
        .Select(m => new Menu
        {
            Id = m.Id,
            Name = m.Name ?? string.Empty,
            Slug = m.Slug ?? string.Empty,
            created_at = m.created_at,
            updated_at = m.updated_at,
            is_deleted = m.is_deleted,
            News = m.News
                .Where(n => !n.is_deleted)
                .Select(n => new News
                {
                    Id = n.Id,
                    Title = n.Title ?? string.Empty,
                    Slug = n.Slug   ?? string.Empty,
                    Content = n.Content,
                    thumbnail = n.thumbnail,
                    Address = n.Address,
                    WardId = n.WardId,
                     created_at = n.created_at,
                    updated_at = n.updated_at,
                    is_deleted = n.is_deleted,
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
                            IsActived = n.Ward.IsActived,
                            Localization = n.Ward.Localization == null
                                        ? null
                                        : new WebsiteLocalization
                                        {
                                            KeyLocalization = n.Ward.KeyLocalization,
                                            Localization =n.Ward.Localization.Localization,
                                            IsActived = n.Ward.Localization.IsActived
                                        }
                        }
                }).ToList()
        });
}


}