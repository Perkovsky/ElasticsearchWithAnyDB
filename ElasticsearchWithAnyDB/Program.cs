using System;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;
using ElasticsearchWithAnyDB.Models;
using ElasticsearchWithAnyDB.Services;
using System.Linq;
using System.Threading.Tasks;

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
			var collectionName = "groups";
			mongoService.CopyToAsync<GroupMongo>(new CollectionCopy
			{
				DbNameFrom = "stozhary_api",
				CollectionNameFrom = collectionName,
				DbNameTo = "nodejs_api",
				CollectionNameTo = collectionName
			}).Wait();
			var countDocuments = mongoService.CountAsync<GroupMongo>("nodejs_api", collectionName).Result;
			printService.PrintInfo($"Total documents copied='{countDocuments}'.");

			printService.PrintInfo($"{Environment.NewLine}Press any key...", false);
			Console.ReadKey();
        }
    }
}
