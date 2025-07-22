using Microsoft.EntityFrameworkCore;
using PersonalWebsite.Data;
using PersonalWebsite.Models;
using System.Collections.Concurrent;

namespace PersonalWebsite.Services
{
    public class PageSettingService(IServiceProvider serviceProvider) : IDataCacheService<PageSetting>
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        private readonly ConcurrentDictionary<string, PageSetting> _cache = new(StringComparer.OrdinalIgnoreCase);

        public async Task<PageSetting> GetLatestAsync(string name)
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            return await dbContext.PageSettings.FirstOrDefaultAsync(s => s.Name.ToLower() == name.ToLower())
                ?? new PageSetting { Name = name };
        }

        public async Task SaveAsync(string name, PageSetting data)
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var dataChanged = false;
            var pageSetting = await dbContext.PageSettings.FirstOrDefaultAsync(p => p.Name.ToLower() == name.ToLower());

            if (pageSetting == null)
            {
                pageSetting = new PageSetting
                {
                    Name = name,
                    IsDisabled = data.IsDisabled,
                    LastModifiedDateTime = DateTime.UtcNow
                };

                dbContext.PageSettings.Add(pageSetting);
                dataChanged = true;
            }
            else
            {
                if (pageSetting.IsDisabled != data.IsDisabled)
                {
                    pageSetting.IsDisabled = data.IsDisabled;
                    pageSetting.LastModifiedDateTime = DateTime.UtcNow;
                    dataChanged = true;
                }
            }

            if (dataChanged)
            {
                await dbContext.SaveChangesAsync();
                RefreshCached(name);
            }
        }

        public async Task<PageSetting> GetCachedAsync(string name)
        {
            if (_cache.TryGetValue(name, out var cached))
            {
                return cached;
            }

            var pageSetting = await GetLatestAsync(name);
            _cache[name] = pageSetting;

            return pageSetting;
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
