using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ElasticsearchWithAnyDB.Models;
using ElasticsearchWithAnyDB.Repositories;

namespace ElasticsearchWithAnyDB
{
    class Program
    {
        private static void SearchAndPrintResult(IRepository repository, string stringSearch, string title)
        {
            var sw = new Stopwatch();
            sw.Start();
            var result = repository.Search(stringSearch);
            var count = result.Count();
            sw.Stop();

            //foreach (var item in res)
            //    Console.WriteLine($"ID: {item.Id}; Name: {item.Name}; Price: {item.PriceB2B}");

            Console.WriteLine($"Title: {title}");
            Console.WriteLine($"Count: {count}");
            Console.WriteLine($"Time : {sw.Elapsed.TotalSeconds} sec");
            Console.WriteLine(new string('-', 40));
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            IRepository repository;
            string searchString = "лопата";

            #region Memory repository

            //Console.WriteLine("Loading memory repository...");
            //repository = new MemoryRepository();
            //SearchAndPrintResult(repository, searchString, "Memory repository search");

            #endregion

            #region EF repository

            //Console.WriteLine("Loading EF repository...");
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

            //Console.WriteLine("Loading FB repository...");
            //repository = new FBRepository();
            //SearchAndPrintResult(repository, searchString, "FB repository");

            #endregion

            #region Elasticsearch repository

            //Console.WriteLine("Loading FB repository...");
            //repository = new FBRepository();
            //SearchAndPrintResult(repository, searchString, "FB repository");

            #endregion


            Console.WriteLine("DONE! PRESS ANY KEY...");
            Console.ReadKey();
        }
    }
}
