using System;
using System.Collections.Generic;
using System.Linq;
using Nest;
using ElasticsearchWithAnyDB.Models;

namespace ElasticsearchWithAnyDB.Repositories
{
    public class ESRepository : BaseRepository, IRepository
    {
        private const string INDEX_NAME = "products";

        private readonly IElasticClient client;

        // Результаты тестирова скорости поиска (метод Search):
        // Тип IEnumerable<Product> :  14,2348343 sec
        // Тип IQueryable<Product>  :  4,8152637 sec
        //
        // P.S.
        //  Для типа IEnumerable<Product> сначала срабатывает полная выборка,
        //  а только потом сам поиск
        //
        public override IQueryable<Product> Products => client.Search<Product>(s => s).Documents.AsQueryable();

        public ESRepository(IElasticClient client)
        {
            this.client = client;
            SeedData();
        }

        public void SaveProduct(Product product) => throw new NotImplementedException();

        public Product DeleteProduct(int productID) => throw new NotImplementedException();

        public void SeedData()
        {
            DeleteIndex();

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
            //string stringSearchToUpper = stringSearch.ToUpper();
            return client.Search<Product>(s => s
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
