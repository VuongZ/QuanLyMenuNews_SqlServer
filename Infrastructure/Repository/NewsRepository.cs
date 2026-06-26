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
    public async Task<News?> GetByIdWithMenusAsync(int id)
    {
    return await Query().FirstOrDefaultAsync(m => m.Id==id);
    }
    public  IEnumerable<News> GetAllWithMenusAsync()
    {
        return Query().AsEnumerable();
    }
    private IQueryable<News> Query()
    {
        return  context.News
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
                        },
                Menu = n.Menu
                .Where(m => m.IsDeleted == false)
                .Select(m => new Menu
                {
                    Id = m.Id,
                    Name = m.Name ?? string.Empty,
                    Slug = m.Slug ?? string.Empty,
                    CreatedAt = m.CreatedAt,
                    UpdatedAt = m.UpdatedAt,
                    IsDeleted = m.IsDeleted
                }).ToList()
            });
    }

}   
                    