using ElasticsearchWithAnyDB.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElasticsearchWithAnyDB.Services
{
	public class MongoService : IMongoService
	{
		private readonly IPrintService printService;
		private readonly IMongoCollection<ProductMongo> products;

		public MongoService(IMongoSettings settings, IPrintService printService)
		{
			this.printService = printService;
			var client = new MongoClient(settings.ConnectionString);
			var database = client.GetDatabase(settings.DatabaseName);
			products = database.GetCollection<ProductMongo>(settings.ProductsCollectionName);
		}

		public async Task<IEnumerable<Product>> GetProductsAsync()
		{
			printService.PrintInfo("Started loading all data from MongoDB...");
			var documents = await products.FindAsync(x => true);
			var result = documents.ToList().Select(x => new Product
			{
				Id = x.Id,
				Name = x.Name,
				BrandProduct = x.BrandProduct,
				Price = (decimal)x.Price,
				Group = $"Group-{x.ParentId}",
				Amount = new Random().Next(1, 100),
				DateCreated = DateTime.Now.AddMonths(new Random().Next(1, 20) * -1)
			});
			printService.PrintInfo("Finished loading all data from MongoDB.");
			return result;
		}
	}
}
