using StackExchange.Redis;

namespace StackExchangeAPI.Web.Services
{
	public interface IRedisCacheService
	{
		IDatabase GetDb(int dbNo);
	}
}
