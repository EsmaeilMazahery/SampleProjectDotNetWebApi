
using ESkimo.DomainLayer.Models;
using ESkimo.Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ESkimo.DataLayer.Mappings
{
    public class PaymentMapping : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.Property(p => p.amount)
             .HasColumnType(ConstantValidations.amountTypeTable);

            builder.HasOne(o => o.member)
               .WithMany(m => m.payments)
               .HasForeignKey(f => f.memberId)
               .IsRequired(true)
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(o => o.factor)
                .WithOne(o => o.payment)
                .HasForeignKey<Payment>(f=>f.factorId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(o => o.memberOrderPeriod)
                .WithMany(o => o.payments)
                .HasForeignKey(f => f.memberOrderPeriodId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}