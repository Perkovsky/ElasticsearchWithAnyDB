using ElasticsearchWithAnyDB.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace ElasticsearchWithAnyDB.Repositories
{
    public class EFRepository : BaseRepository, IRepository
    {
        private ApplicationDbContext context;

        // Результаты тестирова скорости поиска (метод Search):
        // Тип IEnumerable<Product> :  14,2348343 sec
        // Тип IQueryable<Product>  :  4,8152637 sec
        //
        // P.S.
        //  Для типа IEnumerable<Product> сначала срабатывает полная выборка,
        //  а только потом сам поиск
        //
        public override IQueryable<Product> Products => context.Products
                                                               .Include(p => p.BrandProduct);

        public EFRepository(ApplicationDbContext context) => this.context = context;

        public void SaveProduct(Product product)
        {
            if (product.Id == 0)
            {
                context.Products.Add(product);
            }
            else
            {
                Product dbEmpty = context.Products.FirstOrDefault(p => p.Id == product.Id);
                if (dbEmpty != null)
                {
                    dbEmpty.Id = product.Id;
                    dbEmpty.ParentID = product.ParentID;
                    dbEmpty.Name = product.Name;
                    dbEmpty.PriceB2B = product.PriceB2B;
                    dbEmpty.PriceB2C = product.PriceB2C;
                    dbEmpty.PriceWholesale = product.PriceWholesale;
                    dbEmpty.WholesalePacking = product.WholesalePacking;
                    dbEmpty.StatusProduct = product.StatusProduct;
                    dbEmpty.BrandProduct = product.BrandProduct;
                    dbEmpty.VideoUrl = product.VideoUrl;
                    dbEmpty.Description = product.Description;
                    dbEmpty.Keywords = product.Keywords;
                    dbEmpty.Availability = product.Availability;
                    dbEmpty.Delivery = product.Delivery;
                    dbEmpty.LimitOrderDays = product.LimitOrderDays;
                    dbEmpty.Weight = product.Weight;
                    dbEmpty.Volume = product.Volume;
                    dbEmpty.VolumeLimit = product.VolumeLimit;
                    dbEmpty.VolumeIncrementLimit = product.VolumeIncrementLimit;
                    dbEmpty.PromoID = product.PromoID;
                }
            }
            context.SaveChanges();
        }

        public Product DeleteProduct(int productID)
        {
            Product dbEmpty = context.Products.FirstOrDefault(p => p.Id == productID);
            if (dbEmpty != null)
            {
                context.Products.Remove(dbEmpty);
                context.SaveChanges();
            }
            return dbEmpty;
        }

        public void SeedData()
        {
            IEnumerable<Product> loadData = null;
            if (!context.Brands.Any())
            {
                loadData = base.LoadProductsFromFile();
                context.Brands.AddRange(loadData.Select(p => p.BrandProduct).Distinct());
                context.SaveChanges();
            }
            if (!context.Products.Any())
            {
                context.Products.AddRange(loadData ?? base.LoadProductsFromFile());
                context.SaveChanges();
            }
        }
    }
}
