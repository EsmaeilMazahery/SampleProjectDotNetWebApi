
using ESkimo.DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ESkimo.DataLayer.Mappings
{
    public class MemberOrderPeriodMapping : IEntityTypeConfiguration<MemberOrderPeriod>
    {
        public void Configure(EntityTypeBuilder<MemberOrderPeriod> builder)
        {
            builder.HasMany(o => o.factors)
                .WithOne(m => m.memberOrderPeriod)
                .HasForeignKey(f => f.memberOrderPeriodId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(o => o.periodType)
                .WithMany(m => m.memberOrderPeriods)
                .HasForeignKey(f => f.periodTypeId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(o => o.payments)
               .WithOne(o => o.memberOrderPeriod)
               .HasForeignKey(f => f.memberOrderPeriodId)
               .IsRequired(false)
               .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(o => o.member)
                .WithMany(m => m.memberOrderPeriods)
                .HasForeignKey(f => f.memberId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}