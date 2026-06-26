using Domain.entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
    public class MenuConFig : IEntityTypeConfiguration<Menu>
    {
        public void Configure(EntityTypeBuilder<Menu> builder)
        {
            builder.ToTable("menus");
            builder.HasKey(x => x.Id);
            builder.Property(sv => sv.Id).HasColumnName("Menu_Id").ValueGeneratedOnAdd();
            builder.Property(x => x.IsDeleted).HasColumnName("IsDeleted");
            builder.HasIndex(x => x.Slug);
            builder.Property(x => x.Name).HasColumnName("name").HasMaxLength(255);
            builder.Property(x => x.Slug).HasColumnName("slug").HasMaxLength(255);
            builder.Property(x => x.IsDeleted).HasColumnName("is_deleted");
            builder.Property(x => x.CreatedAt).HasColumnName("created_at");
            builder.Property(x => x.UpdatedAt).HasColumnName("updated_at");
            builder.Property(x => x.DateledAt).HasColumnName("deleted_at");
        }   
    }

    
}