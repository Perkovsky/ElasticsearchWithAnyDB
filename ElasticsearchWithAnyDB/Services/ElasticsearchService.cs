using ElasticsearchWithAnyDB.Models;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ElasticsearchWithAnyDB.Services
{
	public class ElasticsearchService
	{
		private readonly string indexName;
		private readonly IElasticClient client;
		private readonly IPrintService printService;
		private readonly IMongoService mongoService;

		public ElasticsearchService(IElasticsearchSettings appSettings, IPrintService printService, IMongoService mongoService)
		{
			this.printService = printService;
			this.mongoService = mongoService;
			indexName = appSettings.IndexName;

			var settings = new ConnectionSettings(new Uri(appSettings.ConnectionString));
			settings.ThrowExceptions(alwaysThrow: true);
			settings.DisableDirectStreaming();
			settings.DefaultIndex(indexName);
			settings.PrettyJson(); // good for DEBUG
			client = new ElasticClient(settings);
		}

		#region Private Methods

		private bool IsIndexExists() => client.IndexExists(indexName).Exists;

		private ICreateIndexResponse CreateIndex()
		{
			if (IsIndexExists()) return null;

			var result = client.CreateIndex(indexName, i => i
				.Mappings(ms => ms
					.Map<Product>(m => m
						.AutoMap()
					)
				)
			);

			if (!result.IsValid)
				printService.PrintError(result.ServerError.Error.ToString());

			return result;
		}

		private void SeedDataInParts(int? first, int partSize)
		{
			IEnumerable<Product> loadData = mongoService.GetProductsAsync().Result;
			int totalItems = first ?? loadData.Count();
			int skipItems = 0;

			while (totalItems > skipItems)
			{
				var part = loadData.Skip(skipItems)
					.Take(partSize);

				skipItems += partSize;
				var result = client.IndexMany(part);

				if (!result.IsValid)
				{
					foreach (var item in result.ItemsWithErrors)
						printService.PrintError($"Failed to index document {item.Id}: {item.Error}");
				}
			}
		}

		#endregion

		//NOTE: При вызове метода 'client.IndexMany(<ALL_PRODUCTS>)' на больших объемах выдает исключение
		// РЕШЕНИЕ: необходимо загружать порциями (размер порции устанавливается опытным путем)
		public void SeedData(int? first = null, int partSize = 10000)
		{
			if (IsIndexExists()) return;

			printService.PrintInfo("Started loading seed data to Elasticsearch...");
			CreateIndex();
			SeedDataInParts(first, partSize);
			printService.PrintInfo("Finished loading seed data to Elasticsearch.");
		}

		public void DeleteIndex()
		{
			if (IsIndexExists())
				client.DeleteIndex(indexName);
		}

		public IEnumerable<Product> Search(string search)
		{
			//TODO: если найденных товаров будет больше чем size, то они обрежутся до size, сделать паджинацию
			int size = 1000;

			return client.Search<Product>(s => s
				.Size(size)
				.Query(q => q
					.QueryString(qs => qs
						.Query(search)
					)
				)
			).Documents;
		}

		public IEnumerable<Product> GetProducts(int parentId)
		{
			//TODO: если товаров в группе будет больше чем size, то они обрежутся до size, сделать паджинацию
			int size = 1000;

			return client.Search<Product>(s => s
					.Size(size)
					.Query(q => q
							.MultiMatch(m => m
								.Fields(f => f
									.Field(p => p.ParentId))
									.Query(parentId.ToString()
								)
							)
					)
		  ).Documents;
		}
	}
}
