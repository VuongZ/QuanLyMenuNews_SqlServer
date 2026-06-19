using Domain.entity;
using Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Menu> Menus { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<MenuNews> MenuNews { get; set; }
        public DbSet<WebsiteLocalization> WebsiteLocalizations { get; set; }

        public DbSet<WebsiteLocalizationWard> WebsiteLocalizationWards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MenuConFig());
            modelBuilder.ApplyConfiguration(new NewsConFig());
            modelBuilder.ApplyConfiguration(new MenuNewsConfig());
            modelBuilder.ApplyConfiguration(new WebsiteLocalizationConfig());
            modelBuilder.ApplyConfiguration(new WebsiteLocalizationWardConfig());

        }

}
