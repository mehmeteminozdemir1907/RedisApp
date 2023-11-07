using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using StackExchangeExampleAPI.Web.Services;

namespace StackExchangeExampleAPI.Web.Controllers
{
	public class ListTypeController : Controller
	{
		private readonly RedisCacheService redisService;
		private readonly IDatabase db;
		private readonly string listKey = "names";

		public ListTypeController(RedisCacheService redisService)
		{
			this.redisService = redisService;
			this.db = this.redisService.GetDb(1); // get db 1
		}

		public async Task<IActionResult> Index()
		{
			var nameList = new List<string>();
			if (this.db.KeyExists(this.listKey))
			{
				var list = await this.db.ListRangeAsync(this.listKey).ConfigureAwait(false); // Returns all data of the key
				list.ToList().ForEach(x => nameList.Add(x.ToString()));
			}

			return View(nameList);
		}

		[HttpPost]
		public async Task<IActionResult> Add(string name)
		{
			await this.db.ListRightPushAsync(this.listKey, name).ConfigureAwait(false); // Appends record to end.
			return RedirectToAction("Index");
		}

		public async Task<IActionResult> Delete(string name)
		{
			await this.db.ListRemoveAsync(this.listKey, name).ConfigureAwait(false); // delete record.
			return RedirectToAction("Index");
		}
	}
}