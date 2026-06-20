using Domain.entity;

namespace Domain.repositories;

public interface IWebsiteLocalizationWardRepo
{
    Task<WebsiteLocalizationWard?> GetByIdAsync(int wardId);
    Task<IEnumerable<WebsiteLocalizationWard>> GetProvincesAsync();
    Task<IEnumerable<WebsiteLocalizationWard>>GetWardsByProvinceIdAsync(int provinceId);
    Task<WebsiteLocalizationWard?> GetProvinceByNameAsync(string provinceName);
    Task<WebsiteLocalizationWard?> GetWardByNameAndProvinceAsync(string wardName,int provinceId);
}