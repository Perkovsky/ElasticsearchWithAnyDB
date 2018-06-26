using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Nest;
using ElasticsearchWithAnyDB.Models;
using ElasticsearchWithAnyDB.Repositories;

namespace ElasticsearchWithAnyDB
{
    class Program
    {
        private static void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }

        private static void Print(string title, ConsoleColor textColor = ConsoleColor.White)
        {
            var defaultTextColor = Console.ForegroundColor;
            Console.ForegroundColor = textColor;
            Console.WriteLine(title);
            Console.ForegroundColor = defaultTextColor;
        }

        private static void PrintSearchResult(string title, int totalItems, int searchItems, double time)
        {
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            ClearCurrentConsoleLine();

            var defaultTextColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(title);
            Console.ForegroundColor = defaultTextColor;
            Console.WriteLine($"Time   : {time} sec");
            Console.WriteLine($"Total  : {totalItems} item(s)");
            Console.WriteLine($"Search : {searchItems} item(s)");
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

            PrintSearchResult(title, repository.TotalItems, count, sw.Elapsed.TotalSeconds);
            if (printExtendedResult)
                PrintExtendedSearchResult(result);
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            IRepository repository;
            string searchString = "лопата";

            IConfigurationRoot config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            #region Memory repository

            //Print("Loading memory repository...");
            //repository = new MemoryRepository();
            //SearchAndPrintResult(repository, searchString, "Memory repository search");

            #endregion

            #region EF repository

            //Print("Loading EF repository...");

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

            Print("Loading ES repository...");

            var settings = new ConnectionSettings(new Uri(config["Elasticsearch:Uri"]));
            settings.ThrowExceptions(alwaysThrow: true);
            settings.DisableDirectStreaming();
            settings.DefaultIndex(ESRepository.INDEX_NAME);
            settings.PrettyJson(); // good for DEBUG

            var client = new ElasticClient(settings);
            
            // search
            repository = new ESRepository(client/*, true*/);
            SearchAndPrintResult(repository, searchString, "ES repository");

            // get products by parent ID
            int parentID = 43029/*197208*/;
            var sw = new Stopwatch();
            sw.Start();
            var result = (repository as ESRepository).GetProductsByParentId(parentID);
            sw.Stop();
            Console.WriteLine();
            PrintSearchResult($"Elasticsearch -> get products by ParentID={parentID}", 
                repository.TotalItems, result.Count(), sw.Elapsed.TotalSeconds);
            PrintExtendedSearchResult(result);

            #endregion

            Print($"{Environment.NewLine}DONE! PRESS ANY KEY...");
            Console.ReadKey();
        }
    }
}
