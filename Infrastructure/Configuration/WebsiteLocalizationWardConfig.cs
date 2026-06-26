using Domain.entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class WebsiteLocalizationWardConfig
    : IEntityTypeConfiguration<WebsiteLocalizationWard>
{
    public void Configure(
        EntityTypeBuilder<WebsiteLocalizationWard> builder)
    {
        builder.ToTable("website_localization_ward_v2");
        builder.HasKey(x => x.WardId);
        builder.Property(x => x.WardId).HasColumnName("ward_id").ValueGeneratedOnAdd();
        builder.Property(x => x.WardPid).HasColumnName("ward_pid");
        builder.Property(x => x.Name).HasColumnName("name").HasMaxLength(64);
        builder.Property(x => x.NameEn).HasColumnName("name_en").HasMaxLength(64);
        builder.Property(x => x.FullName).HasColumnName("full_name").HasMaxLength(96);
        builder.Property(x => x.FullNameEn).HasColumnName("full_name_en").HasMaxLength(96);
        builder.Property(x => x.KeyLocalization).HasColumnName("key_localization").HasMaxLength(32);
        builder.Property(x => x.IsActived).HasColumnName("is_actived");
        builder.HasOne(x => x.Localization).WithMany(x => x.Wards).HasForeignKey(x => x.KeyLocalization).HasPrincipalKey(x => x.KeyLocalization);
        builder.HasIndex(x => x.WardPid);
        builder.HasIndex(x => x.KeyLocalization);
    }
}