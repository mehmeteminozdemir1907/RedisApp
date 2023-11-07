namespace DistributedCache.Web.Services
{
    public interface IRedisCacheService
    {
        Task<string?> GetAsync(string key);

        Task<bool> AddAsync(CacheDto add);

        Task<bool> RemoveAsync(string key);
    }
}
