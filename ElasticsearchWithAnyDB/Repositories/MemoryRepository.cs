using System;
using System.Collections.Generic;
using System.Linq;
using ElasticsearchWithAnyDB.Models;

namespace ElasticsearchWithAnyDB.Repositories
{
    public class MemoryRepository : BaseRepository, IRepository
    {
        private List<Product> products;

        // Результаты тестирова скорости поиска (метод Search):
        // Тип IEnumerable<Product> :  0,37093 sec
        // Тип IQueryable<Product>  :  0,4108401 sec
        //
        // P.S.
        //  Вроде бы для типа IEnumerable<Product> был результат 0,004937 sec,
        //  но при следующем запуске повторить его так и не смог.
        //  Хотя все-равно результат всегда ниже чем для типа IQueryable<Product> 
        //
        public override IQueryable<Product> Products => products.AsQueryable();

        public MemoryRepository() => SeedData();

        public void SaveProduct(Product product) => throw new NotImplementedException();

        public Product DeleteProduct(int productID)
        {
            Product empty = products.FirstOrDefault(p => p.Id == productID);
            if (empty != null)
            {
                products.Remove(empty);
            }
            return empty;
        }

        public void SeedData() => products = base.LoadProductsFromFile() as List<Product>;
    }
}
