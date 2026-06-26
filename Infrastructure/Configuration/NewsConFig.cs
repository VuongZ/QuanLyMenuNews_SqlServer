using Domain.entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
    public class NewsConFig : IEntityTypeConfiguration<News>
    {
        public void Configure(EntityTypeBuilder<News> builder)
        {
            builder.ToTable("news");
            builder.HasKey(x => x.Id);
            builder.Property(sv => sv.Id).HasColumnName("News_Id").ValueGeneratedOnAdd();
            builder.Property(x => x.IsDeleted).HasColumnName("IsDeleted");
            builder.HasIndex(x => x.Slug);
            builder.Property(x => x.Title).HasColumnName("title").HasMaxLength(255);
            builder.Property(x => x.Slug).HasColumnName("slug").HasMaxLength(255);
            builder.Property(x => x.Content).HasColumnName("content");
            builder.Property(x => x.Thumbnail).HasColumnName("Thumbnail").HasMaxLength(255);
            builder.Property(x => x.Address).HasColumnName("address").HasMaxLength(255);
            builder.Property(x => x.WardId).HasColumnName("ward_id");
            builder.HasOne(x => x.Ward).WithMany(x => x.News).HasForeignKey(x => x.WardId);
            builder.HasIndex(x => x.WardId);
            builder.Property(x => x.IsDeleted).HasColumnName("is_deleted");
            builder.Property(x => x.CreatedAt).HasColumnName("created_at");
            builder.Property(x => x.UpdatedAt).HasColumnName("updated_at");
            builder.Property(x => x.DateledAt).HasColumnName("deleted_at");
        }   
    }

    
}