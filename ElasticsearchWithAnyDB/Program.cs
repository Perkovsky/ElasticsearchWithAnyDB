using System;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;
using ElasticsearchWithAnyDB.Models;
using ElasticsearchWithAnyDB.Services;
using System.Collections.Generic;
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

			var printService = new PrintService();
			ElasticsearchSettings elasticsearchSettings = new ElasticsearchSettings();
			config.Bind(nameof(ElasticsearchSettings), elasticsearchSettings);
			var elasticsearchService = new ElasticsearchService(elasticsearchSettings, printService);
			//elasticsearchService.SeedData();

			var targets = new List<string> { "1", "1st", "first", "one", "One", "ONe", "First", "FIRST", "FiRsT", "2nd", "avenue", "Avenue" };
			foreach (var item in targets)
			{
				string search = item;
				var result = elasticsearchService.Search(search);
				printService.PrintInfo(result);
				printService.PrintInfo($"Total products founded by search='{search}': {result.Count()}", false);
				printService.PrintInfo(new string('-', 80) + Environment.NewLine, false);
			}

			printService.PrintInfo($"{Environment.NewLine}Press any key...", false);
			Console.ReadKey();
        }
    }
}
