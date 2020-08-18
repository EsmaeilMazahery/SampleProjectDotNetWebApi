
using ESkimo.DomainLayer.Models;
using ESkimo.Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ESkimo.DataLayer.Mappings
{
    public class PocketPostMapping : IEntityTypeConfiguration<PocketPost>
    {
        public void Configure(EntityTypeBuilder<PocketPost> builder)
        {
            builder.HasOne(o => o.user)
                .WithMany(m => m.pocketPosts)
                .HasForeignKey(f => f.userId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(o => o.userSender)
                .WithMany(m => m.pocketPostSenders)
                .HasForeignKey(f => f.userSenderId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(m => m.factors)
                .WithOne(o => o.pocketPost)
                .HasForeignKey(f => f.pocketPostId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Property(p => p.amount)
              .HasColumnType(ConstantValidations.amountTypeTable);
        }
    }
}