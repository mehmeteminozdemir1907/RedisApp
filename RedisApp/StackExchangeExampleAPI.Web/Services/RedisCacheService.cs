using StackExchange.Redis;

namespace StackExchangeExampleAPI.Web.Services
{
	public class RedisCacheService
	{
		private readonly IConfiguration configuration;
		private readonly string redisHost;
		private readonly string redisPort;
		private ConnectionMultiplexer redis;
		public IDatabase db { get; set; }

		public RedisCacheService(IConfiguration configuration)
        {
			this.configuration = configuration;
			this.redisHost = configuration["Redis:Host"];
			this.redisPort = configuration["Redis:Port"];
		}

		/// <summary>
		/// Connect to redis.
		/// </summary>
		public async void Connect()
		{
			var redisHostUrl = $"{this.redisHost}:{this.redisPort}";
			this.redis = await ConnectionMultiplexer.ConnectAsync(redisHostUrl).ConfigureAwait(false);
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
