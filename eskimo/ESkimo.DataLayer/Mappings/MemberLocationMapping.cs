
using ESkimo.DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ESkimo.DataLayer.Mappings
{
    public class MemberLocationMapping : IEntityTypeConfiguration<MemberLocation>
    {
        public void Configure(EntityTypeBuilder<MemberLocation> builder)
        {
            builder.HasOne(o => o.member)
                .WithMany(m => m.memberLocations)
                .HasForeignKey(f => f.memberId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(o => o.area)
                .WithMany(m => m.memberLocations)
                .HasForeignKey(f => f.areaId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(m => m.factors)
                .WithOne(o => o.memberLocation)
                .HasForeignKey(f => f.memberLocationId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}