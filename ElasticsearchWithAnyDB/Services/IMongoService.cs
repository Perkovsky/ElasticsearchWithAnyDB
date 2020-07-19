using ElasticsearchWithAnyDB.Models;
using System.Threading.Tasks;

namespace ElasticsearchWithAnyDB.Services
{
	public interface IMongoService
	{
		Task CopyToAsync<T>(CollectionCopy copy);

		Task<long> CountAsync<T>(string dbName, string collectionName);
	}
}