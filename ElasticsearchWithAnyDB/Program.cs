using System;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;
using ElasticsearchWithAnyDB.Models;
using ElasticsearchWithAnyDB.Services;
using StackExchange.Redis;

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

			// Redis
			var settings = new RedisSettings();
			config.Bind(nameof(RedisSettings), settings);
			var redis = ConnectionMultiplexer.Connect(settings.ConnectionString);
			IDatabase redisDatabase = redis.GetDatabase();

			var key = "192.168.0.127";
			var result = redisDatabase.StringSet(key, "some simple text as value...");
			var value = redisDatabase.StringGet(key);
			//var value = await db.StringGetAsync(key, flags).ConfigureAwait(false);

			printService.PrintInfo($"{key}: {value}", false);

			printService.PrintInfo($"{Environment.NewLine}Press any key...", false);
			Console.ReadKey();
        }
    }
}
