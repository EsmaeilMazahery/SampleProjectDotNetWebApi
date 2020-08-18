
using ESkimo.DomainLayer.Models;
using ESkimo.Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ESkimo.DataLayer.Mappings
{
    public class DiscountCodeMapping : IEntityTypeConfiguration<DiscountCode>
    {
        public void Configure(EntityTypeBuilder<DiscountCode> builder)
        {
            builder.HasMany(m => m.categories)
                .WithOne(o => o.discountCode)
                .HasForeignKey(f => f.discountCodeId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(m => m.factors)
                .WithOne(o => o.discountCode)
                .HasForeignKey(f => f.discountCodeId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(m => m.brands)
               .WithOne(o => o.discountCode)
               .HasForeignKey(f => f.discountCodeId)
               .IsRequired(true)
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(o => o.member)
                .WithMany(m => m.discountCodes)
                .HasForeignKey(f => f.memberId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(p => p.minPrice)
               .HasColumnType(ConstantValidations.amountTypeTable);

            builder.Property(p => p.maxDiscount)
               .HasColumnType(ConstantValidations.amountTypeTable);

            builder.Property(p => p.percent)
               .HasColumnType(ConstantValidations.amountTypeTable);

            builder.Property(p => p.discount)
               .HasColumnType(ConstantValidations.amountTypeTable);
        }

    }
}