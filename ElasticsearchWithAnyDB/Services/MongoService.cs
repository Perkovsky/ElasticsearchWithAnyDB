using ElasticsearchWithAnyDB.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElasticsearchWithAnyDB.Services
{
	public class MongoService : IMongoService
	{
		private readonly IPrintService printService;
		private readonly IMongoCollection<Product> products;

		public MongoService(IMongoSettings settings, IPrintService printService)
		{
			this.printService = printService;
			var client = new MongoClient(settings.ConnectionString);
			var database = client.GetDatabase(settings.DatabaseName);
			products = database.GetCollection<Product>(settings.ProductsCollectionName);
		}

		public async Task<IEnumerable<Product>> GetProductsAsync()
		{
			printService.PrintInfo("Started loading all data from MongoDB...");
			var documents = await products.FindAsync(x => true);
			var result = documents.ToList();
			printService.PrintInfo("Finished loading all data from MongoDB.");
			return result;
		}
	}
}
