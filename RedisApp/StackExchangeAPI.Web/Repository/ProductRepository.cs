using StackExchange.Redis;
using StackExchangeAPI.Web.Models;
using StackExchangeAPI.Web.Services;
using System.Text.Json;

namespace StackExchangeAPI.Web.Repository
{
	public class ProductRepository : IProductRepository
	{
		private readonly IRedisCacheService redisCacheService;
		private readonly IDatabase db;

		private readonly string redisKey = "products";


		public ProductRepository(IRedisCacheService redisCacheService, IDatabase db)
        {
			this.redisCacheService = redisCacheService;
			this.db = db;
		}

        public async Task<bool> AddAsync(Product product)
		{
			var json = JsonSerializer.Serialize(product);
			return await this.db.HashSetAsync(this.redisKey, product.Id, json).ConfigureAwait(false);
		}

		public async Task<List<Product>> GetAllAsync()
		{
			var data = await this.db.HashGetAllAsync(this.redisKey).ConfigureAwait(false);

			var products = new List<Product>();
			data.ToList().ForEach(x =>
			{
				products.Add(JsonSerializer.Deserialize<Product>(x.Value));
			});

			return products;
		}

		public Product GetById(int id)
		{
			var data = this.db.HashGetAsync(this.redisKey, id).GetAwaiter().GetResult();
			if (data.HasValue)
			{
				return JsonSerializer.Deserialize<Product>(data);
			}

			return null;
		}

		public async Task<bool> RemoveAsync(int id)
		{
			return await this.db.HashDeleteAsync(this.redisKey, id).ConfigureAwait(false);
		}
	}
}
