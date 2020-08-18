
using SP.DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SP.DataLayer.Mappings
{
    public class MemberMapping : IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.HasIndex(b => b.mobile)
                .IsUnique()
                .HasName("UI_Mobile");

            builder.HasIndex(b => b.email)
                    .IsUnique(false)
                    .HasName("UI_Email");

            builder.HasMany(U => U.medias)
                .WithOne(m => m.member)
                .HasForeignKey(m => m.memberId);

            builder.HasMany(U => U.userVisits)
                .WithOne(i => i.member)
                .HasForeignKey(i => i.memberId);

            builder.HasMany(U => U.logUsers)
              .WithOne(i => i.user)
              .HasForeignKey(i => i.userId);

            builder.HasMany(m => m.smsLogs)
                .WithOne(o => o.member)
                .HasForeignKey(f => f.memberId);

            builder.Property(b => b.searchScore).HasColumnType("decimal(18, 2)");
        }
    }
}