using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using StackExchangeExampleAPI.Web.Services;

namespace StackExchangeExampleAPI.Web.Controllers
{
	public class HashTypeController : Controller
	{
		private readonly RedisCacheService redisService;
		private readonly IDatabase db;
		private readonly string listKey = "hashNames";

		public HashTypeController(RedisCacheService redisService)
		{
			this.redisService = redisService;
			this.db = this.redisService.GetDb(1);
		}

		public async Task<IActionResult> Index()
		{
			var dictionary = new Dictionary<string, string>();

			var exist = await this.db.KeyExistsAsync(this.listKey).ConfigureAwait(false);
			if (exist)
			{
				var data = await this.db.HashGetAllAsync(this.listKey).ConfigureAwait(false);
				data.ToList().ForEach(x =>
				{
					dictionary.Add(x.Name, x.Value);
				});
			}

			return View(dictionary);
		}

		[HttpPost]
		public async Task<IActionResult> Add(string key, string value)
		{
			await this.db.HashSetAsync(this.listKey, key, value).ConfigureAwait(false);

			return RedirectToAction("Index");
		}

		public async Task<IActionResult> Delete(string key)
		{
			await this.db.HashDeleteAsync(this.listKey, key).ConfigureAwait(false);

			return RedirectToAction("Index");
		}
	}
}
