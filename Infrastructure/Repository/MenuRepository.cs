
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
     public async Task<Menu?> GetByIdWithNewsAsync(int id)
    {
        return await GetAllWithNewsQuery().FirstOrDefaultAsync(m => m.Id == id);
            
    }
    public IEnumerable<Menu> GetAllWithNewsAsync()
    {
        return  GetAllWithNewsQuery().AsEnumerable();
     
    }
    private IQueryable<Menu> GetAllWithNewsQuery()
{
    return context.Menus
        .Where(m => !m.IsDeleted)
        .Select(m => new Menu
        {
            Id = m.Id,
            Name = m.Name ?? string.Empty,
            Slug = m.Slug ?? string.Empty,
            CreatedAt = m.CreatedAt,
            UpdatedAt = m.UpdatedAt,
            IsDeleted = m.IsDeleted,
            News = m.News
                .Where(n => !n.IsDeleted)
                .Select(n => new News
                {
                    Id = n.Id,
                    Title = n.Title ?? string.Empty,
                    Slug = n.Slug   ?? string.Empty,
                    Content = n.Content,
                    Thumbnail = n.Thumbnail,
                    Address = n.Address,
                    WardId = n.WardId,
                    CreatedAt = n.CreatedAt,
                    UpdatedAt = n.UpdatedAt,
                    IsDeleted = n.IsDeleted,
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