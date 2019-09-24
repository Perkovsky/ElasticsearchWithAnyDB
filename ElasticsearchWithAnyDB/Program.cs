using System;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;
using ElasticsearchWithAnyDB.Models;
using ElasticsearchWithAnyDB.Services;
using System.Linq;

namespace ElasticsearchWithAnyDB
{
	class Program
    {
		static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

			// PrintService
			var printService = new PrintService();
			// MongoService
			MongoSettings mongoSettings = new MongoSettings();
			config.Bind(nameof(MongoSettings), mongoSettings);
			var mongoService = new MongoService(mongoSettings, printService);
			// ElasticsearchService
			ElasticsearchSettings elasticsearchSettings = new ElasticsearchSettings();
			config.Bind(nameof(ElasticsearchSettings), elasticsearchSettings);
			var elasticsearchService = new ElasticsearchService(elasticsearchSettings, printService, mongoService);
			//elasticsearchService.DeleteIndex(); return;
			elasticsearchService.SeedData(4990, 4990);

			string search = "шампунь";

			var result = elasticsearchService.Autocomlete(search);
			printService.PrintInfo(result);

			printService.PrintInfo($"{Environment.NewLine}Press any key...", false);
			Console.ReadKey();
        }
    }
}
