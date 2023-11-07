namespace StackExchangeAPI.Web.Models
{
	public class Product
	{
        public int Id { get; set; }

		public required string Name { get; set; }

		public int Price { get; set; }
	}
}
