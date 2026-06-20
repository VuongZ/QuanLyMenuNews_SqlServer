using Application.DTO;
using Domain.entity;
using Domain.repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;
public class NewsRepository : Repository<News> , INewsRepo , INewsQueryRepository
{
    public NewsRepository(AppDbContext context) : base(context)
    {
        
    }

    public Task<News?> GetBySlugAsync(string slug)
    {
        return _context.News.FirstOrDefaultAsync(x => x.Slug == slug && x.is_deleted == false);
    }
    public async Task<NewsResponseDto?> GetByIdWithMenusAsync(int id)
{
    return await Query()
        .FirstOrDefaultAsync(m => m.Id==id);
}
    public  IAsyncEnumerable<NewsResponseDto> GetAllWithMenusAsync()
    {
      return Query().AsAsyncEnumerable();
    }
    private IQueryable<NewsResponseDto> Query()
    {
          return  _context.News
            .Where(n => n.is_deleted == false)
            .Select(n => new NewsResponseDto
            {
                Id = n.Id,
                Title = n.Title ?? string.Empty,
                Slug = n.Slug ?? string.Empty,
                Content = n.Content,
                Thumbnail = n.thumbnail,
                Address = n.Address,
                WardId = n.WardId,
                FullAddress = n.Ward == null
                ? n.Address
                : (n.Address ?? string.Empty)
                + ", "
                + (n.Ward.FullName ?? string.Empty),
            WardInfo = n.Ward == null
                ? null
                : new WardInfoResponseDto
                {
                    WardId = n.Ward.WardId,
                    WardPid = n.Ward.WardPid,
                    Name = n.Ward.Name,
                    NameEn = n.Ward.NameEn,
                    FullName = n.Ward.FullName ?? string.Empty,
                    FullNameEn = n.Ward.FullNameEn,
                    Country = n.Ward.Localization!.Localization ?? string.Empty,
                    WardParent = new WardParentResponseDto
                            {
                                WardId = n.Ward.WardPid,

                                Name = n.Ward.FullName != null &&
                                    n.Ward.FullName.Contains(",")
                                    ? n.Ward.FullName
                                        .Substring(n.Ward.FullName.IndexOf(",") + 1)
                                        .Trim()
                                    : string.Empty,

                                NameEn = n.Ward.FullNameEn != null &&
                                        n.Ward.FullNameEn.Contains(",")
                                    ? n.Ward.FullNameEn
                                        .Substring(n.Ward.FullNameEn.IndexOf(",") + 1)
                                        .Trim()
                                    : null,
                                    Country = n.Ward.Localization!.Localization ?? string.Empty,
                            }
                        
                },
                Menus = n.Menu
                .Where(m => m.is_deleted == false)
                .Select(m => new MenuBasicResponseDto
                {
                    Id = m.Id,
                    Name = m.Name ?? string.Empty,
                    Slug = m.Slug ?? string.Empty,  
                })
            
            });
    }

    public Task<News?> GetByIdWithMenusForUpdateAsync(int id)
    {
        throw new NotImplementedException();
    }
}   
                    