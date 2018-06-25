using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ElasticsearchWithAnyDB.Models;
using ElasticsearchWithAnyDB.Repositories;

namespace ElasticsearchWithAnyDB
{
    class Program
    {
        private static void Print(string title, ConsoleColor textColor = ConsoleColor.White)
        {
            var defaultTextColor = Console.ForegroundColor;
            Console.ForegroundColor = textColor;
            Console.WriteLine(title);
            Console.ForegroundColor = defaultTextColor;
        }

        private static void PrintSearchResult(string title, int count, double time)
        {
            var defaultTextColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Title: {title}");
            Console.ForegroundColor = defaultTextColor;
            Console.WriteLine($"Time : {time} sec");
            Console.WriteLine($"Count: {count}");
        }

        private static void PrintExtendedSearchResult(IEnumerable<Product> searchResult)
        {
            Console.WriteLine("Item(s):");
            foreach (var item in searchResult)
                Console.WriteLine($"\tID: {item.Id} \tParentID: {item.ParentID}   \tPrice: {item.PriceB2B} \tName: {HttpUtility.HtmlDecode(item.Name)}");
        }

        private static void SearchAndPrintResult(IRepository repository, string stringSearch, string title, bool printExtendedResult = false)
        {
            var sw = new Stopwatch();
            sw.Start();
            var result = repository.Search(stringSearch);
            var count = result.Count();
            sw.Stop();

            PrintSearchResult(title, count, sw.Elapsed.TotalSeconds);
            if (printExtendedResult)
                PrintExtendedSearchResult(result);
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            IRepository repository;
            string searchString = "лопата";

            #region Memory repository

            Print("Loading memory repository...");
            repository = new MemoryRepository();
            SearchAndPrintResult(repository, searchString, "Memory repository search", true);

            #endregion

            #region EF repository

            //Print("Loading EF repository...");
            //IConfigurationRoot config = new ConfigurationBuilder()
            //    .SetBasePath(Directory.GetCurrentDirectory())
            //    .AddJsonFile("appsettings.json")
            //    .Build();

            //var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            //var options = optionsBuilder.UseSqlServer(config.GetConnectionString("StozharyProducts")).Options;
            //using (ApplicationDbContext context = new ApplicationDbContext(options))
            //{
            //    repository = new EFRepository(context);
            //    SearchAndPrintResult(repository, searchString, "EF repository");
            //}

            #endregion

            #region FB repository

            //Print("Loading FB repository...");
            //repository = new FBRepository();
            //SearchAndPrintResult(repository, searchString, "FB repository");

            #endregion

            #region Elasticsearch repository

            //Print("Loading FB repository...");
            //repository = new FBRepository();
            //SearchAndPrintResult(repository, searchString, "FB repository");

            #endregion

            Print($"{Environment.NewLine}DONE! PRESS ANY KEY...");
            Console.ReadKey();
        }
    }
}
