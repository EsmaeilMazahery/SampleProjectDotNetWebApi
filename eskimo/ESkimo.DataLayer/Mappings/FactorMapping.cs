
using ESkimo.DomainLayer.Models;
using ESkimo.Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ESkimo.DataLayer.Mappings
{
    public class FactorMapping : IEntityTypeConfiguration<Factor>
    {
        public void Configure(EntityTypeBuilder<Factor> builder)
        {
            builder.Property(p => p.amount)
              .HasColumnType(ConstantValidations.amountTypeTable);

            builder.Property(p => p.amountSend)
              .HasColumnType(ConstantValidations.amountTypeTable);

            builder.Property(p => p.discountOfFactor)
              .HasColumnType(ConstantValidations.amountTypeTable);

            builder.Property(p => p.discountOfCode)
              .HasColumnType(ConstantValidations.amountTypeTable);

            builder.Property(p => p.discountOfPeriod)
              .HasColumnType(ConstantValidations.amountTypeTable);

            builder.Property(p => p.tax)
              .HasColumnType(ConstantValidations.amountTypeTable);

            builder.HasOne(o => o.periodType)
                .WithMany(m => m.factors)
                .HasForeignKey(f => f.periodTypeId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(o => o.member)
                .WithMany(m => m.factors)
                .HasForeignKey(f => f.memberId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(o => o.discountFactor)
                .WithMany(m => m.factors)
                .HasForeignKey(f => f.discountFactorId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(o => o.discountCode)
                .WithMany(m => m.factors)
                .HasForeignKey(f => f.discountCodeId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(o => o.memberLocation)
                .WithMany(m => m.factors)
                .HasForeignKey(f => f.memberLocationId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(o => o.pocketPost)
                .WithMany(m => m.factors)
                .HasForeignKey(f => f.pocketPostId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(m => m.factorItems)
                .WithOne(o => o.factor)
                .HasForeignKey(f => f.factorId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(o => o.payment)
               .WithOne(o => o.factor)
               .HasForeignKey<Factor>(f => f.paymentId)
               .IsRequired(false)
               .OnDelete(DeleteBehavior.SetNull);
        }
    }
}