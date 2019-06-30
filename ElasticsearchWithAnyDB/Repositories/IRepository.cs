using System.Collections.Generic;
using System.Linq;
using ElasticsearchWithAnyDB.Models;

namespace ElasticsearchWithAnyDB.Repositories
{
    //TODO: в реальном проекте все методы реализации сделать Async
    public interface IRepository
    {
        IQueryable<Product> Products { get; }

        int TotalItems { get; }

        void SaveProduct(Product product);

        Product DeleteProduct(int productID);

        IEnumerable<Product> Search(string searchString);

        IEnumerable<Product> GetProducts(int parentId);

        void SeedData();
    }
}
