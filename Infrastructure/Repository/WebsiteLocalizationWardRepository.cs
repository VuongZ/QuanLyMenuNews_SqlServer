using Domain.entity;
using Domain.repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class WebsiteLocalizationWardRepository
    : IWebsiteLocalizationWardRepo
{
    private readonly AppDbContext context;

    public WebsiteLocalizationWardRepository(
        AppDbContext context)
    {
        this.context = context;
    }

    public async Task<IEnumerable<WebsiteLocalizationWard>> GetAllAsync()
    {
        return await context.WebsiteLocalizationWards
            .AsNoTracking()
            .Include(x => x.Localization)
            .Where(x => x.IsActived)
            .ToListAsync();
    }

    public async Task<WebsiteLocalizationWard?>GetByIdAsync(int wardId)
    {
        return await context.WebsiteLocalizationWards
            .AsNoTracking()
            .Include(x => x.Localization)
            .FirstOrDefaultAsync(x =>
                x.WardId == wardId &&
                x.IsActived);
    }

    public async Task<IEnumerable<WebsiteLocalizationWard>>GetProvincesAsync()
    {
        return await context.WebsiteLocalizationWards
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

    public async Task<IEnumerable<WebsiteLocalizationWard>> GetWardsByProvinceIdAsync(int provinceId)
    {
        return await context.WebsiteLocalizationWards
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
        public async Task<WebsiteLocalizationWard?>  GetProvinceByNameAsync(string provinceName)
            {
                var normalizedName = provinceName.Trim().ToLower();
                return await context.WebsiteLocalizationWards
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x =>
                        x.WardPid == 0 &&
                        x.IsActived &&
                        (
                            x.Name.ToLower() == normalizedName ||
                            x.FullName.ToLower() == normalizedName
                        ));
            }
        public async Task<WebsiteLocalizationWard?> GetWardByNameAndProvinceAsync( string wardName,int provinceId)
            {
                var normalizedName = wardName.Trim().ToLower();

                return await context.WebsiteLocalizationWards
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x =>
                        x.WardPid == provinceId &&
                        x.IsActived &&
                        (
                            x.Name.ToLower() == normalizedName ||
                            x.FullName.ToLower() == normalizedName
                        ));
            }
}
