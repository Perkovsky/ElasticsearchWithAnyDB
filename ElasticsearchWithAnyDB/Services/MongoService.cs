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
		private const string DB_NAME = "stozhary_api";

		private readonly IPrintService _printService;
		private readonly MongoClient _client;

		public MongoService(IMongoSettings settings, IPrintService printService)
		{
			_printService = printService;
			_client = new MongoClient(settings.ConnectionString);
		}

		public async Task<IEnumerable<Group>> GetGroups()
		{
			var db = _client.GetDatabase(DB_NAME);
			var collection = db.GetCollection<GroupMongo>("groups");
			var result = await collection.FindAsync(x => true);

			return result.ToList()
				.Select(n => new Group
				{
					Id = n.Id,
					ParentId = n.ParentId,
					Name = n.Name,
					Keywords = n.Keywords,
					PhotoUrl = n.PhotoUrl
				});
		}

		public async Task<IEnumerable<Brand>> GetBrands(int[] parentIds)
		{
			var db = _client.GetDatabase(DB_NAME);
			var collection = db.GetCollection<ProductMongo>("products");
			var result = await collection.FindAsync(x => parentIds.Contains(x.ParentId));
			var comparer = new ComparerById<Brand>();

			return result.ToList()
				.Select(n => new Brand
				{
					Id = n.BrandProduct.Id,
					Name = n.BrandProduct.Name
				})
				.Distinct(comparer);
		}

		public async Task<IEnumerable<Product>> GetProducts(int[] parentIds)
		{
			var db = _client.GetDatabase(DB_NAME);
			var collection = db.GetCollection<ProductMongo>("products");
			var result = await collection.FindAsync(x => parentIds.Contains(x.ParentId));

			return result.ToList()
				.Select(n => new Product
				{
					Id = n.Id,
					Name = n.Name,
					BrandId = n.BrandProduct.Id,
					Availability = n.Availability,
					Description = n.Description,
					Keywords = n.Keywords,
					LimitOrderDays = n.LimitOrderDays,
					ParentId = n.ParentId,
					PhotoUrl = n.PhotoUrl,
					PhotoUrlBig = n.PhotoUrlBig,
					Price = n.Price,
					StatusProduct = n.StatusProduct,
					VideoUrl = n.VideoUrl,
					WholesalePacking = n.WholesalePacking
				});
		}
	}
}
