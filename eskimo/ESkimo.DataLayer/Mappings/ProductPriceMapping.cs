
using ESkimo.DomainLayer.Models;
using ESkimo.Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ESkimo.DataLayer.Mappings
{
    public class ProductPriceMapping : IEntityTypeConfiguration<ProductPrice>
    {
        public void Configure(EntityTypeBuilder<ProductPrice> builder)
        {
            builder.HasMany(m=>m.factorItems)
                .WithOne(o=>o.productPrice)
                .HasForeignKey(f=>f.productPriceId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(r => r.product)
                .WithMany(m => m.productPrices)
                .HasForeignKey(f => f.productId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);

            

            builder.Property(p => p.amount)
                .HasColumnType(ConstantValidations.amountTypeTable);
        }

    }
}