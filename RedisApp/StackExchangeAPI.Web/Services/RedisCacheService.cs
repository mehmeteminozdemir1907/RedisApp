using StackExchange.Redis;

namespace StackExchangeAPI.Web.Services
{
	public class RedisCacheService : IRedisCacheService
	{
		private ConnectionMultiplexer redis;

		public RedisCacheService(string redisHostUrl)
		{
			this.redis = ConnectionMultiplexer.Connect(redisHostUrl);
		}

		/// <summary>
		/// Get redis database by Database Number.
		/// </summary>
		/// <param name="dbNo">Database Number.</param>
		/// <returns></returns>
		public IDatabase GetDb(int dbNo)
		{
			return this.redis.GetDatabase(dbNo);
		}
	}
}
