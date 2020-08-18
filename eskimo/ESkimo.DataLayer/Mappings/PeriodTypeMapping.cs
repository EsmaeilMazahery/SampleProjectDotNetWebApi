
using ESkimo.DomainLayer.Models;
using ESkimo.Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ESkimo.DataLayer.Mappings
{
    public class PeriodTypeMapping : IEntityTypeConfiguration<PeriodType>
    {
        public void Configure(EntityTypeBuilder<PeriodType> builder)
        {
            builder.Property(p => p.percentDiscount)
              .HasColumnType(ConstantValidations.amountTypeTable);

            builder.Property(p => p.maxDiscount)
              .HasColumnType(ConstantValidations.amountTypeTable);

            builder.HasMany(m => m.areas)
                .WithOne(o => o.periodType)
                .HasForeignKey(f => f.periodTypeId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(m => m.factors)
                .WithOne(o => o.periodType)
                .HasForeignKey(f => f.periodTypeId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(m => m.memberOrderPeriods)
                .WithOne(o => o.periodType)
                .HasForeignKey(f => f.periodTypeId)
                 .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}