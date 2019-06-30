using System.Collections.Generic;
using System.Threading.Tasks;
using ElasticsearchWithAnyDB.Models;

namespace ElasticsearchWithAnyDB.Services
{
	public interface IMongoService
	{
		Task<IEnumerable<Product>> GetProductsAsync();
	}
}