using StackExchangeAPI.Web.Models;

namespace StackExchangeAPI.Web
{
	public class ProductDto
	{
        public Product Product { get; set; }

        public List<Product> Products { get; set; }
    }
}
