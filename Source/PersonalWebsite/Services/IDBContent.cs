namespace PersonalWebsite.Services
{
    public interface IDBContent<T>
    {
        Task<T> GetLatestAsync(string key);
        Task SaveAsync(string key, string value);
        Task<T> GetCachedAsync(string key);
        void RefreshCached(string key);
        void ClearCached(string? key);
    }
}
