using System;
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

        public override int TotalItems => (int)client.Search<Product>(s => s).Total;

        public ESRepository(IElasticClient client, bool deleteIndex = false)
        {
            this.client = client;

            if (deleteIndex)
                DeleteIndex();

            SeedData();
        }

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
            //TODO: если найденных товаров будет больше чем size, то они обрежутся до size, сделать паджинацию
            int size = 1000;

            return client.Search<Product>(s => s
                .Size(size)
                .Query(q => q
                    .QueryString(qs => qs
                        .Query(stringSearch)
                    )
                )
            ).Documents;
        }

        public IEnumerable<Product> GetProductsByParentId(int parentId)
        {
            //TODO: если товаров в группе будет больше чем size, то они обрежутся до size, сделать паджинацию
            int size = 1000;

            return client.Search<Product>(s => s
                    .Size(size)
                    .Query(q => q
                            .MultiMatch(m => m
                                .Fields(f => f
                                    .Field(p => p.ParentID))
                                    .Query(parentId.ToString()
                                )
                            )
                    )
          ).Documents;
        }

        #region Not Implemented Methods and Properties

        public override IQueryable<Product> Products => throw new NotImplementedException();
        public void SaveProduct(Product product) => throw new NotImplementedException();
        public Product DeleteProduct(int productID) => throw new NotImplementedException();

        #endregion

        #region Privates Methods

        private bool IsIndexExists() => client.IndexExists(INDEX_NAME).Exists;

        private ICreateIndexResponse CreateIndex()
        {
            if (IsIndexExists()) return null;

            var result = client.CreateIndex(INDEX_NAME, i => i
                .Mappings(ms => ms
                    .Map<Product>(m => m
                        .AutoMap()
                    )
                )
            );

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

        #endregion
    }
}
