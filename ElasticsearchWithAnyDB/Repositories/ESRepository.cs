﻿using System;
using System.Collections.Generic;
using System.Linq;
using Nest;
using ElasticsearchWithAnyDB.Models;

namespace ElasticsearchWithAnyDB.Repositories
{
    public class ESRepository : BaseRepository, IRepository
    {
        public const string INDEX_NAME = "products";

        private readonly IElasticClient client;

        public override int TotalItems => 0;

        public override IQueryable<Product> Products => throw new NotImplementedException();

        public ESRepository(IElasticClient client, bool deleteIndex = false)
        {
            this.client = client;

            if (deleteIndex)
                DeleteIndex();

            SeedData();
        }

        public void SaveProduct(Product product) => throw new NotImplementedException();

        public Product DeleteProduct(int productID) => throw new NotImplementedException();

        public void SeedData()
        {
            if (IsIndexExists()) return;

            CreateIndex();

            IEnumerable<Product> loadData = base.LoadProductsFromFile();
            var result = client.IndexMany(loadData);

            if (!result.IsValid)
            {
                var defaultTextColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                foreach (var item in result.ItemsWithErrors)
                    Console.WriteLine("Failed to index document {0}: {1}", item.Id, item.Error);

                Console.WriteLine(result.DebugInformation);
                Console.ForegroundColor = defaultTextColor;
            }
        }

        public override IEnumerable<Product> Search(string stringSearch)
        {
            return client.Search<Product>(s => s.Size(1000)
                .Query(q => q
                    .QueryString(qs => qs.Query(stringSearch))))
                        .Documents;
        }

        private bool IsIndexExists() => client.IndexExists(INDEX_NAME).Exists;

        private ICreateIndexResponse CreateIndex()
        {
            if (IsIndexExists()) return null;

            var result = client.CreateIndex(INDEX_NAME, i =>
                i.Mappings(ms => ms
                    .Map<Product>(m => m.AutoMap())));

            if (!result.IsValid)
            {
                var defaultTextColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.ServerError.Error);
                Console.ForegroundColor = defaultTextColor;
            }

            return result;
        }

        private void DeleteIndex()
        {
            if (IsIndexExists())
                client.DeleteIndex(INDEX_NAME);
        }
    }
}