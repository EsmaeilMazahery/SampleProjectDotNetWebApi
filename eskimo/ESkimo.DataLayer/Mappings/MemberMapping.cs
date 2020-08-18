
using ESkimo.DomainLayer.Models;
using ESkimo.Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ESkimo.DataLayer.Mappings
{
    public class MemberMapping : IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.HasIndex(b => b.mobile)
                .IsUnique()
                .HasName("UI_Mobile");

            builder.HasIndex(b => b.email)
                    .IsUnique()
                    .HasName("UI_Email");

            builder.Property(p => p.amount)
              .HasColumnType(ConstantValidations.amountTypeTable);

            builder.Property(p => p.sumPayment)
             .HasColumnType(ConstantValidations.amountTypeTable);

            builder.Property(p => p.sumFactors)
             .HasColumnType(ConstantValidations.amountTypeTable);

            builder.HasMany(m => m.memberLocations)
                .WithOne(o => o.member)
                .HasForeignKey(f => f.memberId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(m => m.comments)
                .WithOne(o => o.member)
                .HasForeignKey(f => f.memberId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(m => m.discountCodes)
                .WithOne(o => o.member)
                .HasForeignKey(f => f.memberId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(m => m.factors)
                .WithOne(o => o.member)
                .HasForeignKey(f => f.memberId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(m => m.smsLogs)
                .WithOne(o => o.member)
                .HasForeignKey(f => f.memberId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        }

    }
}