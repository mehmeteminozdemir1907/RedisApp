using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using StackExchangeExampleAPI.Web.Services;

namespace StackExchangeExampleAPI.Web.Controllers
{
	public class SetTypeController : Controller
	{
		private readonly RedisCacheService redisService;
		private readonly IDatabase db;
		private readonly string listKey = "setNames";

		public SetTypeController(RedisCacheService redisService)
		{
			this.redisService = redisService;
			this.db = this.redisService.GetDb(2);
		}

		public async Task<IActionResult> Index()
		{
			var names = new HashSet<string>(); // Compared to a list, the values it contains are unique and unordered.

			var exist = await this.db.KeyExistsAsync(listKey).ConfigureAwait(false);
			if (exist)
			{
				var data = await this.db.SetMembersAsync(listKey).ConfigureAwait(false);
				data?.ToList().ForEach(x =>
				{
					names.Add(x.ToString());
				});
			}

			return View(names);
		}

		[HttpPost]
		public async Task<IActionResult> Add(string name)
		{
			await this.db.KeyExpireAsync(this.listKey, DateTime.Now.AddMinutes(5)).ConfigureAwait(false); // We define a 5-minute timeout for the given key. It gives 5 minutes of life each time it runs.

			await this.db.SetAddAsync(this.listKey, name).ConfigureAwait(false);

			return RedirectToAction("Index");
		}

		public async Task<IActionResult> Delete(string name)
		{
			await this.db.SetRemoveAsync(this.listKey, name).ConfigureAwait(false);
			return RedirectToAction("Index");
		}
	}
}
