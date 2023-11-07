using DistributedCache.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace DistributedCache.Web.Controllers
{
	public class ProductsController : Controller
    {
        private readonly IRedisCacheService redisCacheService;

        public ProductsController(IRedisCacheService redisCache)
        {
            this.redisCacheService = redisCache;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Product product)
        {
            if (product is null)
            {
                return this.NoContent();
            }
            var cacheAddDto = new CacheDto
            {
                Key = product.Id.ToString(),
                Value = product.Name,
			};

            var result = await this.redisCacheService.AddAsync(cacheAddDto).ConfigureAwait(false);

            if (!result)
            {
                return this.BadRequest();
            }

            return this.RedirectToAction("Index");
        }

        public async Task<IActionResult> Get(string key)
        {
            var data = await this.redisCacheService.GetAsync(key).ConfigureAwait(false);
            TempData["Key"] = key;
            TempData["Data"] = data ?? "Record is not found";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int key)
        {
            var result = await this.redisCacheService.RemoveAsync(key.ToString()).ConfigureAwait(false);

            if (!result)
            {
                return this.BadRequest();
            }

			return this.RedirectToAction("Index");
		}
    }
}
