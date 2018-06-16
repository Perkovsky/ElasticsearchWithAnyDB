using Microsoft.EntityFrameworkCore;
using ElasticsearchWithAnyDB.Models.EntityConfigurations;

namespace ElasticsearchWithAnyDB.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Brand> Brands { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new BrandEntityTypeConfiguration());
            builder.ApplyConfiguration(new ProductEntityTypeConfiguration());
        }
    }
}
