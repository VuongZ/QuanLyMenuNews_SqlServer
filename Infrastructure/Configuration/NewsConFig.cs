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
            builder.Property(sv => sv.Id)
            .HasColumnName("News_Id")
            .ValueGeneratedOnAdd();
            builder.Property(x => x.is_deleted)
            .HasColumnName("is_deleted")
            .HasColumnType("bit");
            builder.HasIndex(x => x.Slug)
            .IsUnique();
            builder.Property(x => x.Title).HasMaxLength(255).IsRequired();
            builder.Property(x => x.Slug).HasMaxLength(255).IsRequired();
            builder.Property(x => x.Content).IsRequired();
            builder.Property(x => x.thumbnail).HasMaxLength(255);

        }   
    }

    
}