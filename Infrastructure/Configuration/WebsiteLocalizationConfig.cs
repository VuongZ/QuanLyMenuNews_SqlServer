using Domain.entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class WebsiteLocalizationConfig
    : IEntityTypeConfiguration<WebsiteLocalization>
{
    public void Configure(
        EntityTypeBuilder<WebsiteLocalization> builder)
    {
        builder.ToTable("website_localization");
        builder.HasKey(x => x.KeyLocalization);
        builder.Property(x => x.KeyLocalization).HasColumnName("key_localization").HasMaxLength(32);
        builder.Property(x => x.Localization).HasColumnName("localization").HasMaxLength(32);
        builder.Property(x => x.IsActived).HasColumnName("is_actived");
    }
}