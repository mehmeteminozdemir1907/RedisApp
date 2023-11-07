using Microsoft.AspNetCore.Mvc;
using StackExchangeAPI.Web.Repository;

namespace StackExchangeAPI.Web.Controllers
{
	public class ProductsController : Controller
	{
		private readonly IProductRepository productRepository;

		public ProductsController(IProductRepository productRepository)
        {
			this.productRepository = productRepository;
		}

        public async Task<IActionResult> Index()
		{
			var products = await this.productRepository.GetAllAsync().ConfigureAwait(false);

			var model = new ProductDto
			{
				Products = products,
			};

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Add(ProductDto dto)
		{
			if (dto.Product == null)
			{
				return NoContent();
			}

			var result = await this.productRepository.AddAsync(dto.Product).ConfigureAwait(false);

			if (!result)
			{
				return BadRequest();
			}

			return RedirectToAction("Index");
		}

		public async Task<IActionResult> Delete(int id)
		{
			var result = await this.productRepository.RemoveAsync(id).ConfigureAwait(false);

			if (!result)
			{
				return BadRequest();
			}

			return RedirectToAction("Index");
		}
	}
}
