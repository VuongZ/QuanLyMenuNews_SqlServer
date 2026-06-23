using Domain.entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
    public class MenuNewsConfig : IEntityTypeConfiguration<Menu>
    {
        public void Configure(EntityTypeBuilder<Menu> builder)
        {
            builder.HasMany(menu => menu.News)
                .WithMany(news => news.Menu)
                .UsingEntity<MenuNews>(
                    right => right
                        .HasOne(x => x.News)
                        .WithMany()
                        .HasForeignKey(x => x.NewsId)
                        .OnDelete(DeleteBehavior.NoAction),
                    left => left
                        .HasOne(x => x.Menu)
                        .WithMany()
                        .HasForeignKey(x => x.MenuId)
                        .OnDelete(DeleteBehavior.NoAction),
                    join =>
                    {
                        join.ToTable("Menu_News");
                        join.HasKey(x => new { x.MenuId, x.NewsId });
                        join.Property(x => x.MenuId)
                            .HasColumnName("Menu_Id");
                        join.Property(x => x.NewsId)
                            .HasColumnName("News_Id");
                    });
        }
    }
}
