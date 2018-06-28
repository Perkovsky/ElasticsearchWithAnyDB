using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Firebase.Database;
using Firebase.Database.Query;
using ElasticsearchWithAnyDB.Models;

namespace ElasticsearchWithAnyDB.Repositories
{
    public class FBRepository : BaseRepository, IRepository
    {
        static readonly string BASE_URL = "https://testdb-32619.firebaseio.com/";
        static readonly string AUTH = "TJVs7OWpIsiKOFmmhEYhyd5cz1qPvPqz99xuf2CS";
        static readonly string ROOT_KEY_PRODUCTS = "products";

        private FirebaseClient client;

        public override IQueryable<Product> Products => GetProductsAsync().Result.AsQueryable();

        public FBRepository()
        {
            client = new FirebaseClient(BASE_URL, new FirebaseOptions
            {
                AuthTokenAsyncFactory = () => Task.FromResult(AUTH)
            });

            SeedData();
        }

        public void SaveProduct(Product product) => SaveProductAsync(product).Wait();

        public Product DeleteProduct(int productID) => DeleteProductAsync(productID).Result;

        public void SeedData() => SeedDataAsync().Wait();

        public async Task SeedDataAsync()
        {
            bool isExists = await IsExists();
            if (isExists) return;

            int i = 0;
            var products = base.LoadProductsFromFile();
            foreach (var item in products)
            {
                await SaveProductAsync(item);
                Debug.WriteLine($"#{++i} ID: {item.Id}; Name: {item.Name}");
            }
        }

        private async Task<bool> IsExists()
        {
            var result = await client.Child(ROOT_KEY_PRODUCTS)
                                     .OnceSingleAsync<Product>();

            return (result != null) ? true : false;
        }

        private async Task SaveProductAsync(Product product)
        {
            string childKey = product.Id.ToString();
            string data = JsonConvert.SerializeObject(product);

            await client.Child(ROOT_KEY_PRODUCTS)
                        .Child(childKey)
                        .PutAsync(data);
        }

        private async Task<IEnumerable<Product>> GetProductsAsync()
        {
            var products = await client.Child(ROOT_KEY_PRODUCTS)
                                       .OnceAsync<Product>();

            return products.Select(u => u.Object);
        }

        private async Task<Product> DeleteProductAsync(int productID)
        {
            string childKey = productID.ToString();
            await client.Child(ROOT_KEY_PRODUCTS)
                        .Child(childKey)
                        .DeleteAsync();

            return new Product();
        }
    }
}
