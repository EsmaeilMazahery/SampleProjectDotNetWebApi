
using ESkimo.DomainLayer.Models;
using ESkimo.Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ESkimo.DataLayer.Mappings
{
    public class DiscountFactorMapping : IEntityTypeConfiguration<DiscountFactor>
    {
        public void Configure(EntityTypeBuilder<DiscountFactor> builder)
        {
            builder.HasMany(m => m.factors)
                .WithOne(o => o.discountFactor)
                .HasForeignKey(f => f.discountFactorId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Property(p => p.minPrice)
              .HasColumnType(ConstantValidations.amountTypeTable);

            builder.Property(p => p.percent)
              .HasColumnType(ConstantValidations.amountTypeTable);

            builder.Property(p => p.maxDiscount)
              .HasColumnType(ConstantValidations.amountTypeTable);

            builder.Property(p => p.discount)
              .HasColumnType(ConstantValidations.amountTypeTable);

        }

    }
}