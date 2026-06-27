using Domain.entity;

namespace Domain.repositories;

public interface IWebsiteLocalizationRepo
{
    Task<WebsiteLocalization?> GetByKeyAsync(string keyLocalization);
}