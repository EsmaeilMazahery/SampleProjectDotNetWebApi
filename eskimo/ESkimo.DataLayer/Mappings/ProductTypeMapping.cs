
using ESkimo.DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ESkimo.DataLayer.Mappings
{
    public class ProductTypeMapping : IEntityTypeConfiguration<ProductType>
    {
        public void Configure(EntityTypeBuilder<ProductType> builder)
        {
            builder.HasMany(m => m.products)
                .WithOne(o => o.productType)
                .HasForeignKey(f => f.productTypeId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(m => m.categories)
                .WithOne(o => o.productType)
                .HasForeignKey(f => f.productTypeId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}