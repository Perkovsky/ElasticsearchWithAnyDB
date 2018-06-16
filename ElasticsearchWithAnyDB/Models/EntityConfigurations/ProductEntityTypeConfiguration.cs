using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElasticsearchWithAnyDB.Models.EntityConfigurations
{
    class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.Property(ci => ci.Id)
                   .ForSqlServerUseSequenceHiLo("products_hilo")
                   .IsRequired();
            builder.Property(ci => ci.ParentID).IsRequired();
            builder.Property(ci => ci.Name).IsRequired();
            builder.Property(ci => ci.PriceB2B).IsRequired();
            builder.Property(ci => ci.PriceB2C).IsRequired();
            builder.Property(ci => ci.PriceWholesale).IsRequired(false);
            builder.Property(ci => ci.WholesalePacking).IsRequired();
            builder.Property(ci => ci.StatusProduct).IsRequired();
            builder.HasOne(ci => ci.BrandProduct)
                   .WithMany()
                   .HasForeignKey(ci => ci.BrandProductId);
            builder.Property(ci => ci.VideoUrl).IsRequired();
            builder.Property(ci => ci.Description).IsRequired();
            builder.Property(ci => ci.Keywords).IsRequired();
            builder.Property(ci => ci.Availability).IsRequired();
            builder.Property(ci => ci.Delivery).IsRequired();
            builder.Property(ci => ci.LimitOrderDays).IsRequired(false);
            builder.Property(ci => ci.Weight).IsRequired(false);
            builder.Property(ci => ci.Volume).IsRequired(false);
            builder.Property(ci => ci.VolumeLimit).IsRequired(false);
            builder.Property(ci => ci.VolumeIncrementLimit).IsRequired(false);
            builder.Property(ci => ci.PromoID).IsRequired();
        }
    }
}
