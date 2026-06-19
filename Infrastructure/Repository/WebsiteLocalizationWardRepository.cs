using Domain.entity;
using Domain.repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class WebsiteLocalizationWardRepository
    : IWebsiteLocalizationWardRepo
{
    private readonly AppDbContext _context;

    public WebsiteLocalizationWardRepository(
        AppDbContext context)
    {
        _context = context;
    }

    public async Task<WebsiteLocalizationWard?>
        GetByIdAsync(int wardId)
    {
        return await _context.WebsiteLocalizationWards
            .FirstOrDefaultAsync(x =>
                x.WardId == wardId &&
                x.IsActived);
    }

    public async Task<IEnumerable<WebsiteLocalizationWard>>
        GetProvincesAsync()
    {
        return await _context.WebsiteLocalizationWards
            .Where(x =>
                x.WardPid == 0 &&
                x.IsActived)
            .Select(x => new WebsiteLocalizationWard
            {
                WardId = x.WardId,
                WardPid = x.WardPid,
                Name = x.Name,
                NameEn = x.NameEn,
                FullName = x.FullName,
                FullNameEn = x.FullNameEn,
                KeyLocalization = x.KeyLocalization,
                IsActived = x.IsActived
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<WebsiteLocalizationWard>>
        GetWardsByProvinceIdAsync(int provinceId)
    {
        return await _context.WebsiteLocalizationWards
            .Where(x =>
                x.WardPid == provinceId &&
                x.IsActived)
            .Select(x => new WebsiteLocalizationWard
            {
                WardId = x.WardId,
                WardPid = x.WardPid,
                Name = x.Name,
                NameEn = x.NameEn,
                FullName = x.FullName,
                FullNameEn = x.FullNameEn,
                KeyLocalization = x.KeyLocalization,
                IsActived = x.IsActived
            })
            .ToListAsync();
    }
}