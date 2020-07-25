using ElasticsearchWithAnyDB.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElasticsearchWithAnyDB.Services
{
	public interface IMongoService
	{
		Task<IEnumerable<Group>> GetGroups();

		Task<IEnumerable<Brand>> GetBrands(int[] parentId);

		Task<IEnumerable<Product>> GetProducts(int[] parentId);
	}
}