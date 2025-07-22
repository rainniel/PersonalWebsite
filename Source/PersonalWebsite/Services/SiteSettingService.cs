using Microsoft.EntityFrameworkCore;
using PersonalWebsite.Data;
using PersonalWebsite.Models;
using System.Collections.Concurrent;

namespace PersonalWebsite.Services
{
    public class SiteSettingService(IServiceProvider serviceProvider) : IDataCacheService<SiteSetting>
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        private readonly ConcurrentDictionary<string, SiteSetting> _cache = new(StringComparer.OrdinalIgnoreCase);

        public async Task<SiteSetting> GetLatestAsync(string name)
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            return await dbContext.SiteSettings.FirstOrDefaultAsync(s => s.Name.ToLower() == name.ToLower())
                ?? new SiteSetting { Name = name };
        }

        public async Task SaveAsync(string name, SiteSetting data)
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var dataChanged = false;
            var siteSetting = await dbContext.SiteSettings.FirstOrDefaultAsync(p => p.Name.ToLower() == name.ToLower());

            if (siteSetting == null)
            {
                siteSetting = new SiteSetting
                {
                    Name = name,
                    Value = data.Value,
                    LastModifiedDateTime = DateTime.UtcNow
                };

                dbContext.SiteSettings.Add(siteSetting);
                dataChanged = true;
            }
            else
            {
                if (siteSetting.Value != data.Value)
                {
                    siteSetting.Value = data.Value;
                    siteSetting.LastModifiedDateTime = DateTime.UtcNow;
                    dataChanged = true;
                }
            }

            if (dataChanged)
            {
                await dbContext.SaveChangesAsync();
                RefreshCached(name);
            }
        }

        public async Task<SiteSetting> GetCachedAsync(string name)
        {
            if (_cache.TryGetValue(name, out var cached))
            {
                return cached;
            }

            var siteSetting = await GetLatestAsync(name);
            _cache[name] = siteSetting;

            return siteSetting;
        }

        public void RefreshCached(string name) => ClearCached(name);

        public void ClearCached(string? name = null)
        {
            if (!string.IsNullOrEmpty(name))
            {
                _cache.TryRemove(name, out _);
            }
            else
            {
                _cache.Clear();
            }
        }
    }
}
