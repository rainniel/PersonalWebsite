using Microsoft.EntityFrameworkCore;
using PersonalWebsite.Data;
using PersonalWebsite.Models;
using System.Collections.Concurrent;

namespace PersonalWebsite.Services
{
    public class SocialMediaService(IServiceProvider serviceProvider) : IDataCacheService<SocialMedia>
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        private readonly ConcurrentDictionary<string, SocialMedia> _cache = new(StringComparer.OrdinalIgnoreCase);

        public async Task<SocialMedia> GetLatestAsync(string name)
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            return await dbContext.SocialMedias.FirstOrDefaultAsync(s => s.Name.ToLower() == name.ToLower())
                ?? new SocialMedia { Name = name };
        }

        public async Task SaveAsync(string name, SocialMedia data)
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var dataChanged = false;
            var socialMedia = await dbContext.SocialMedias.FirstOrDefaultAsync(p => p.Name.ToLower() == name.ToLower());

            if (socialMedia == null)
            {
                socialMedia = new SocialMedia
                {
                    Name = name,
                    URL = data.URL,
                    IsHidden = data.IsHidden,
                    LastModifiedDateTime = DateTime.UtcNow
                };

                dbContext.SocialMedias.Add(socialMedia);
                dataChanged = true;
            }
            else
            {
                if (socialMedia.URL != data.URL || socialMedia.IsHidden != data.IsHidden)
                {
                    socialMedia.URL = data.URL;
                    socialMedia.IsHidden = data.IsHidden;
                    socialMedia.LastModifiedDateTime = DateTime.UtcNow;
                    dataChanged = true;
                }
            }

            if (dataChanged)
            {
                await dbContext.SaveChangesAsync();
                RefreshCached(name);
            }
        }

        public async Task<SocialMedia> GetCachedAsync(string name)
        {
            if (_cache.TryGetValue(name, out var cached))
            {
                return cached;
            }

            var socialMedia = await GetLatestAsync(name);
            _cache[name] = socialMedia;

            return socialMedia;
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
