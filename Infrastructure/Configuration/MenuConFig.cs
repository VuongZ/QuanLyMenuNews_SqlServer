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
            builder.Property(sv => sv.Id)
            .HasColumnName("Menu_Id")
            .ValueGeneratedOnAdd();
            builder.Property(x => x.is_deleted)
            .HasColumnName("is_deleted")
            .HasColumnType("bit");
            builder.HasIndex(x => x.Slug)
            .IsUnique();
            builder.Property(x => x.Name).HasMaxLength(255).IsRequired();
            builder.Property(x => x.Slug).HasMaxLength(255).IsRequired();
        }   
    }

    
}