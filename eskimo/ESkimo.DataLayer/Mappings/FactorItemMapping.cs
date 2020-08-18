
using ESkimo.DomainLayer.Models;
using ESkimo.Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ESkimo.DataLayer.Mappings
{
    public class FactorItemMapping : IEntityTypeConfiguration<FactorItem>
    {
        public void Configure(EntityTypeBuilder<FactorItem> builder)
        {
            builder.HasOne(o => o.productPrice)
                .WithMany(m => m.factorItems)
                .HasForeignKey(f => f.productPriceId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(o => o.productPrice)
                .WithMany(m => m.factorItems)
                .HasForeignKey(f => f.productPriceId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(p => p.amountBasePerItem)
              .HasColumnType(ConstantValidations.amountTypeTable);

            builder.Property(p => p.amountPerItem)
              .HasColumnType(ConstantValidations.amountTypeTable);

            builder.Property(p => p.amount)
              .HasColumnType(ConstantValidations.amountTypeTable);
        }

    }
}