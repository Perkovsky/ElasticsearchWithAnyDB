using ElasticsearchWithAnyDB.Models;
using Nest;
using System;
using System.Collections.Generic;

namespace ElasticsearchWithAnyDB.Services
{
	public class ElasticsearchService
	{
		private readonly string indexName;
		private readonly IElasticClient client;
		private readonly IPrintService printService;

		public ElasticsearchService(IElasticsearchSettings appSettings, IPrintService printService)
		{
			this.printService = printService;
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
			DeleteIndex();

			var result = client.CreateIndex(indexName, c => c
				.Settings(s => s
					.Analysis(a => a
						.TokenFilters(tf => tf
							.Stop("building_stop", sw => sw
								.StopWords("avenue")
							)
						)
						.CharFilters(cf => cf
							.Mapping("building", mca => mca
								.Mappings(new[]
								{
									"1 => first",
									"1st => first",
									"1-st => first",
									"one => first",
									"One => first"
								})
							)
						)
						.Analyzers(an => an
							.Custom("index_building", ca => ca
								.CharFilters("html_strip", "building")
								.Tokenizer("standard")
								.Filters("lowercase", "stop", "building_stop")
								
							)
							.Custom("search_building", ca => ca
								.CharFilters("building")
								.Tokenizer("standard")
								.Filters("lowercase", "stop", "building_stop")
							)
						)
					)
				)
				.Mappings( ms => ms
					.Map<Product>(mm => mm
						.AutoMap()
						.Properties(p => p
							.Text(t => t
								.Name(n => n.Name)
								.Analyzer("index_building")
								.SearchAnalyzer("search_building")
							)
						)
					)
				)
			);

			if (!result.IsValid)
				printService.PrintError(result.ServerError.Error.ToString());

			return result;
		}

		#endregion

		public void SeedData()
		{
			printService.PrintInfo("Started loading seed data to Elasticsearch...");
			CreateIndex();
			var loadData = new List<Product>
			{
				new Product { Id = 777771, Name = "1 avenue"},
				new Product { Id = 777772, Name = "1st avenue"},
				new Product { Id = 777773, Name = "1-st avenue"},
				new Product { Id = 777774, Name = "First avenue"},
				new Product { Id = 777775, Name = "first avenue"},
				new Product { Id = 777776, Name = "One avenue"},
				new Product { Id = 777777, Name = "one avenue"},
				new Product { Id = 777778, Name = "2 avenue"},
				new Product { Id = 777779, Name = "2nd avenue"},
				new Product { Id = 777780, Name = "2-nd avenue"},
				new Product { Id = 777781, Name = "Second avenue"},
				new Product { Id = 777782, Name = "second avenue"},
				new Product { Id = 777783, Name = "Two avenue"},
				new Product { Id = 777784, Name = "two avenue"}
			};

			var result = client.IndexMany(loadData);
			if (!result.IsValid)
			{
				foreach (var item in result.ItemsWithErrors)
					printService.PrintError($"Failed to index document {item.Id}: {item.Error}");
			}
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
