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

			using (ApplicationContext context = new ApplicationContext())
			{
				printService.PrintInfo($"Started copying groups from MongoDB to PostgreSQL...");
				var groups = mongoService.GetGroups().Result;
				context.Groups.AddRange(groups);
				context.SaveChanges();
				printService.PrintInfo($"Total groups copied: {groups.Count()}.");

				int[] parentIds = new int[] { 74848, 1093, 23029, 785, 137358 };

				printService.PrintInfo($"Started copying brands from MongoDB to PostgreSQL...");
				var brands = mongoService.GetBrands(parentIds).Result;
				context.Brands.AddRange(brands);
				context.SaveChanges();
				printService.PrintInfo($"Total brands copied: {brands.Count()}.");

				printService.PrintInfo($"Started copying products from MongoDB to PostgreSQL...");
				var products = mongoService.GetProducts(parentIds).Result;
				context.Products.AddRange(products);
				context.SaveChanges();
				printService.PrintInfo($"Total products copied: {products.Count()}.");

			}

			printService.PrintInfo($"{Environment.NewLine}Press any key...", false);
			Console.ReadKey();
        }
    }
}
