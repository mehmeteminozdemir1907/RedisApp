using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using StackExchangeExampleAPI.Web.Services;

namespace StackExchangeExampleAPI.Web.Controllers
{
	public class SortedSetTypeController : Controller
	{
		private readonly RedisCacheService redisService;
		private readonly IDatabase db;
		private readonly string listKey = "sortedNames";

		public SortedSetTypeController(RedisCacheService redisService)
		{
			this.redisService = redisService;
			this.db = this.redisService.GetDb(2);
		}

		public async Task<IActionResult> Index()
		{
			var list = new HashSet<string>();
			var exists = await this.db.KeyExistsAsync(listKey).ConfigureAwait(false);
			if (exists)
			{
				// With the SortedSetScan method, data is fetched according to score order.
				//this.db.SortedSetScan(this.listKey).ToList()?.ForEach(x =>
				//{
				//	list.Add(x.ToString());
				//});

				var datas = await this.db.SortedSetRangeByRankAsync(this.listKey, order: Order.Descending).ConfigureAwait(false); // SortedSetRangeByRankAsync Fetching is done by sorting with the method.

				datas.ToList()?.ForEach(x =>
				{
					list.Add(x.ToString());
				});
			}

			return View(list);
		}

		[HttpPost]
		public async Task<IActionResult> Add(string name, int score)
		{
			await this.db.KeyExpireAsync(this.listKey, DateTime.Now.AddMinutes(1)).ConfigureAwait(false);
			await this.db.SortedSetAddAsync(this.listKey, name, score).ConfigureAwait(false); // It saves sequentially according to the incoming score value. Sorting can be not only integer but also float type and decimal.

			return RedirectToAction("Index");
		}

		public async Task<IActionResult> Delete(string name)
		{
			await this.db.SortedSetRemoveAsync(this.listKey, name).ConfigureAwait(false);

			return RedirectToAction("Index");
		}
	}
}
