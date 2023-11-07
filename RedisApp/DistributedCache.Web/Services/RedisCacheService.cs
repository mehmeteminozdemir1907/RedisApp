using Microsoft.Extensions.Caching.Distributed;

namespace DistributedCache.Web.Services
{
	public class RedisCacheService : IRedisCacheService
	{
		private readonly IDistributedCache distributedCache;
		private readonly DistributedCacheEntryOptions cacheEntryOptions = new DistributedCacheEntryOptions();

		public RedisCacheService(IDistributedCache distributedCache)
		{
			this.distributedCache = distributedCache;
			cacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(10);
		}

		public async Task<bool> AddAsync(CacheDto add)
		{
			await this.distributedCache.SetStringAsync(add.Key, add.Value, cacheEntryOptions).ConfigureAwait(false);
			return true;
		}

		public async Task<string?> GetAsync(string key)
		{
			try
			{
				return await this.distributedCache.GetStringAsync(key).ConfigureAwait(false);
			}
			catch
			{
				return null;
			}
		}

		public async Task<bool> RemoveAsync(string key)
		{
			await this.distributedCache.RemoveAsync(key).ConfigureAwait(false);
			return true;
		}
	}
}
