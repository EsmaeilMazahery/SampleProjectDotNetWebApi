
using ESkimo.DomainLayer.Models;
using ESkimo.Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ESkimo.DataLayer.Mappings
{
    public class ProductMapping : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasOne(o => o.brand)
                .WithMany(m => m.products)
                .HasForeignKey(f => f.brandId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(o => o.category)
                  .WithMany(m => m.products)
                  .HasForeignKey(f => f.categoryId)
                  .IsRequired(true)
                  .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(o => o.productType)
                  .WithMany(m => m.products)
                  .HasForeignKey(f => f.productTypeId)
                  .IsRequired(true)
                  .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(m => m.productPrices)
                .WithOne(o => o.product)
                .HasForeignKey(f => f.productId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(m => m.productPriceWholesales)
                .WithOne(o => o.product)
                .HasForeignKey(f => f.productId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(m => m.comments)
                .WithOne(o => o.product)
                .HasForeignKey(f => f.productId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(p => p.amountBase)
                .HasColumnType(ConstantValidations.amountTypeTable);
        }

    }
}