using System;
using System.Collections.Generic;
using System.Linq;
using ElasticsearchWithAnyDB.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ElasticsearchWithAnyDB.Repositories
{
    public class MongoRepository : BaseRepository, IRepository
    {
        public const string COLLECTION_NAME = "Products";

        private readonly IMongoDatabase database;
        private readonly FilterDefinition<Product> emptyFilter = new FilterDefinitionBuilder<Product>().Empty;

        public new IMongoCollection<Product> Products => database.GetCollection<Product>(COLLECTION_NAME);

        public override int TotalItems => (int)Products.CountDocuments(emptyFilter);

        public MongoRepository(string connectionString, bool deleteCollection = false)
        {
            var connection = new MongoUrlBuilder(connectionString);
            MongoClient client = new MongoClient(connectionString);
            database = client.GetDatabase(connection.DatabaseName);

            if (deleteCollection)
                DeleteCollection();

            SeedData();
        }

        public async void SeedData()
        {
            if (TotalItems > 0) return;

            IEnumerable<Product> loadData = base.LoadProductsFromFile();
            await Products.InsertManyAsync(loadData);
        }

        public override IEnumerable<Product> Search(string stringSearch)
        {
            string s = stringSearch.ToUpper();
            return Products.Find(p => p.Name.ToUpper().Contains(s) 
                                        || p.Keywords.ToUpper().Contains(s))
                           .ToList();
        }

        public override IEnumerable<Product> GetProducts(int parentId)
        {
            //NOTE: названия полей чувствительны к регистру (пример: ParentID != ParentId)
            return Products.Find(new BsonDocument("ParentID", parentId)).ToList();
        }

        private void DeleteCollection() => database.DropCollection(COLLECTION_NAME);

        #region Not Implemented Methods and Properties

        public void SaveProduct(Product product) => throw new NotImplementedException();
        public Product DeleteProduct(int productID) => throw new NotImplementedException();

        #endregion
    }
}
