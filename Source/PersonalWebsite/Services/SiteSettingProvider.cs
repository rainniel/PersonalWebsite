using Microsoft.EntityFrameworkCore;
using PersonalWebsite.Data;
using PersonalWebsite.Models;
using System.Collections.Concurrent;

namespace PersonalWebsite.Services
{
    public class SiteSettingProvider(IServiceProvider serviceProvider) : IDBContent<SiteSetting>
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        private readonly ConcurrentDictionary<string, SiteSetting> _cache = new(StringComparer.OrdinalIgnoreCase);

        public async Task<SiteSetting> GetLatestAsync(string settingName)
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            return await dbContext.SiteSettings.FirstOrDefaultAsync(s => s.SettingName.ToLower() == settingName.ToLower())
                ?? new SiteSetting { SettingName = settingName };
        }

        public async Task SaveAsync(string settingName, string value)
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var valueChanged = false;
            var siteSetting = await dbContext.SiteSettings.FirstOrDefaultAsync(p => p.SettingName.ToLower() == settingName.ToLower());

            if (siteSetting == null)
            {
                siteSetting = new SiteSetting
                {
                    SettingName = settingName,
                    Value = value,
                    LastModifiedDateTime = DateTime.UtcNow
                };

                dbContext.SiteSettings.Add(siteSetting);
                valueChanged = true;
            }
            else
            {
                if (!siteSetting.Value.Equals(value))
                {
                    siteSetting.Value = value;
                    siteSetting.LastModifiedDateTime = DateTime.UtcNow;
                    valueChanged = true;
                }
            }

            if (valueChanged)
            {
                await dbContext.SaveChangesAsync();
                RefreshCached(settingName);
            }
        }

        public async Task<SiteSetting> GetCachedAsync(string settingName)
        {
            if (_cache.TryGetValue(settingName, out var cached))
            {
                return cached;
            }

            var siteSetting = await GetLatestAsync(settingName);
            _cache[settingName] = siteSetting;

            return siteSetting;
        }

        public void RefreshCached(string settingName) => ClearCached(settingName);

        public void ClearCached(string? settingName = null)
        {
            if (!string.IsNullOrEmpty(settingName))
            {
                _cache.TryRemove(settingName, out _);
            }
            else
            {
                _cache.Clear();
            }
        }
    }
}
