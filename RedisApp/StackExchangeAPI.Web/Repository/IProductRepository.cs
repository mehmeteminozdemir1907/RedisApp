using StackExchangeAPI.Web.Models;

namespace StackExchangeAPI.Web.Repository
{
	public interface IProductRepository
	{
		Product GetById(int id);

		Task<List<Product>> GetAllAsync();

		Task<bool> AddAsync(Product product);

		Task<bool> RemoveAsync(int id);
	}
}
