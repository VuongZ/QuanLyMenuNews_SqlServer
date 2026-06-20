using Application.DTO;
using Application.Interfaces;
using Domain.entity;
using Domain.repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;
public class MenuRepository : Repository<Menu> , IMenuRepo ,IMenuQueryRepository
{
    public MenuRepository(AppDbContext context) : base(context)
    {}
          public async Task<Menu?> GetBySlugAsync(string slug)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.Slug == slug && x.is_deleted == false);
    }
     public async Task<MenuResponseDto?> GetByIdWithNewsAsync(int id)
    {
        return await GetAllWithNewsQuery()
            .FirstOrDefaultAsync(m => m.Id == id);
            
    }

    public IAsyncEnumerable<MenuResponseDto> GetAllWithNewsAsync()
    {
        return  GetAllWithNewsQuery().AsAsyncEnumerable();
     
    }
    private IQueryable<MenuResponseDto> GetAllWithNewsQuery()
{
    return _context.Menus
        .Where(m => !m.is_deleted)
        .Select(m => new MenuResponseDto
        {
            Id = m.Id,
            Name = m.Name ?? string.Empty,
            Slug = m.Slug ?? string.Empty,

            News = m.News
                .Where(n => !n.is_deleted)
                .Select(n => new NewsResponseDto
                {
                    Id = n.Id,
                    Title = n.Title ?? string.Empty,
                    Slug = n.Slug   ?? string.Empty,
                    Content = n.Content,
                    Thumbnail = n.thumbnail,
                    Address = n.Address,
                    WardId = n.WardId,

                    FullAddress = n.Ward == null
                        ? n.Address
                        : n.Address + ", " + n.Ward.FullName,

                    WardInfo = n.Ward == null
                        ? null
                        : new WardInfoResponseDto
                        {
                            WardId = n.Ward.WardId,
                            WardPid = n.Ward.WardPid,
                            Name = n.Ward.Name,
                            NameEn = n.Ward.NameEn,
                            FullName = n.Ward.FullName,
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
                        }
                })
        });
}

    public Task<Menu?> GetByIdWithNewsForUpdateAsync(int id)
    {
        throw new NotImplementedException();
    }
}