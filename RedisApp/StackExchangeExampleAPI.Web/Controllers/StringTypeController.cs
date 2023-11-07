using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using StackExchangeExampleAPI.Web.Services;

namespace StackExchangeExampleAPI.Web.Controllers
{
	public class StringTypeController : Controller
	{
		private readonly RedisCacheService redisService;
		private readonly IDatabase db;

		public StringTypeController(RedisCacheService redisService)
        {
			this.redisService = redisService;
			this.db = this.redisService.GetDb(0);
		}

        public IActionResult Index()
		{
			this.db.StringSet("name", "Mehmet Emin Özdemir");
			this.db.StringSet("ziyaretci", 100);

			return View();
		}

		public async Task<IActionResult> Show()
		{
			var value = await this.db.StringGetAsync("name").ConfigureAwait(false);

			if (value.HasValue)
			{
				ViewBag.value = value;
			}

			await this.db.StringIncrementAsync("ziyaretci", 10).ConfigureAwait(false); // It increases the data in the given key by 10.
			await this.db.StringDecrementAsync("ziyaretci", 1).ConfigureAwait(false); // It reduces the data in the given key by 1.

			var imageByte = Array.Empty<byte>();

			await this.db.StringSetAsync("image", imageByte).ConfigureAwait(false); // Image upload.

			return View();
		}
	}
}
