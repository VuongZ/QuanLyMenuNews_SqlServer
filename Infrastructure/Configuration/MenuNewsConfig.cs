using Domain.entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
    public class MenuNewsConfig : IEntityTypeConfiguration<MenuNews>
    {
        public void Configure(EntityTypeBuilder<MenuNews> builder)
        {
            builder.ToTable("Menu_News");
            builder.HasKey(mn => new { mn.MenuId, mn.NewsId });
            builder.Property(mn => mn.MenuId).HasColumnName("menu_id");
            builder.Property(mn => mn.NewsId).HasColumnName("news_id");
            builder.HasOne(mn => mn.Menu).WithMany(m => m.MenuNews).HasForeignKey(mn => mn.MenuId);
            builder.HasOne(mn => mn.News).WithMany(n => n.MenuNews).HasForeignKey(mn => mn.NewsId);
        }
    }
}