using Microsoft.EntityFrameworkCore;
using PersonalWebsite.Models;

namespace PersonalWebsite.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<SiteSetting> SiteSettings { get; set; }
        public DbSet<SocialMedia> SocialMedias { get; set; }
        public DbSet<PageContent> PageContents { get; set; }
        public DbSet<PageSetting> PageSettings { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }
    }
}
