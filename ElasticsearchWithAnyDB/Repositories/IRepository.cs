using System.Collections.Generic;
using System.Linq;
using ElasticsearchWithAnyDB.Models;

namespace ElasticsearchWithAnyDB.Repositories
{
    public interface IRepository
    {
        IQueryable<Product> Products { get; }

        void SaveProduct(Product product);

        Product DeleteProduct(int productID);

        IEnumerable<Product> Search(string searchString);

        void SeedData();
    }
}
