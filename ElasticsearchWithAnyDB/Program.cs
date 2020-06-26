using System;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;
using ElasticsearchWithAnyDB.Models;
using ElasticsearchWithAnyDB.Services;
using System.Linq;
using ServiceStack.Redis;

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
			
			// redis
			var redisSettings = new RedisSettings();
			config.Bind(nameof(RedisSettings), redisSettings);

            // ConnectionString example: "password@host:6379"
            var manager = new RedisManagerPool(redisSettings.ConnectionString);
            using (var client = manager.GetClient())
            {
				// set
				client.Set("fist-name", "John");
				client.Set("last-name", "Dou");
				client.Set("age", 25);
				//get
				printService.PrintInfo($"Fist name: {client.Get<string>("fist-name")}");
                printService.PrintInfo($"Last name: {client.Get<string>("last-name")}");
                printService.PrintInfo($"Age: {client.Get<int>("age")}");
            }

			printService.PrintInfo($"{Environment.NewLine}Press any key...", false);
			Console.ReadKey();
        }
    }
}
