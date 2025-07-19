using Microsoft.EntityFrameworkCore;
using PersonalWebsite.Data;
using PersonalWebsite.Models;
using System.Collections.Concurrent;

namespace PersonalWebsite.Services
{
    public class PageContentProvider(IServiceProvider serviceProvider) : IDBContent<PageContent>
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        private readonly ConcurrentDictionary<string, PageContent> _cache = new(StringComparer.OrdinalIgnoreCase);

        public async Task<PageContent> GetLatestAsync(string pageName)
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            return await dbContext.PageContents.FirstOrDefaultAsync(p => p.PageName.ToLower() == pageName.ToLower())
                ?? new PageContent { PageName = pageName };
        }

        public async Task SaveAsync(string pageName, string value)
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var pageContent = await dbContext.PageContents.FirstOrDefaultAsync(p => p.PageName.ToLower() == pageName.ToLower());

            if (pageContent == null)
            {
                pageContent = new PageContent
                {
                    PageName = pageName,
                    Content = value,
                    LastModified = DateTime.UtcNow
                };

                dbContext.PageContents.Add(pageContent);
            }
            else
            {
                pageContent.Content = value;
                pageContent.LastModified = DateTime.UtcNow;
            }

            await dbContext.SaveChangesAsync();
            RefreshCached(pageName);
        }

        public async Task<PageContent> GetCachedAsync(string pageName)
        {
            if (_cache.TryGetValue(pageName, out var cached))
            {
                return cached;
            }

            var content = await GetLatestAsync(pageName);
            _cache[pageName] = content;

            return content;
        }

        public void RefreshCached(string pageName) => ClearCached(pageName);

        public void ClearCached(string? pageName)
        {
            if (!string.IsNullOrEmpty(pageName))
            {
                _cache.TryRemove(pageName, out _);
            }
            else
            {
                _cache.Clear();
            }
        }
    }
}
