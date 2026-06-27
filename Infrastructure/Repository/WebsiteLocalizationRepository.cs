using Domain.entity;
using Domain.repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class WebsiteLocalizationRepository : IWebsiteLocalizationRepo
{
    private readonly AppDbContext context;
    public WebsiteLocalizationRepository(AppDbContext context)
    {
        this.context = context;
    }
    public Task<WebsiteLocalization?> GetByKeyAsync(string keyLocalization)
    {
        return context.WebsiteLocalizations
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.KeyLocalization == keyLocalization && x.IsActived);
        
    }
}