using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElasticsearchWithAnyDB.Models.EntityConfigurations
{
    class BrandEntityTypeConfiguration : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder.ToTable("Brands");

            builder.HasKey(ci => ci.Id);
            builder.Property(ci => ci.Id)
                   .ForSqlServerUseSequenceHiLo("brands_hilo")
                   .IsRequired();
            builder.Property(cb => cb.Name).IsRequired();
            builder.Property(cb => cb.Code1C).IsRequired();
        }
    }
}
