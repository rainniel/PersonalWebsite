namespace PersonalWebsite.Services
{
    public interface IDataCacheService<T>
    {
        Task<T> GetLatestAsync(string name);
        Task SaveAsync(string name, T data);
        Task<T> GetCachedAsync(string name);
        void RefreshCached(string name);
        void ClearCached(string? name);
    }
}
