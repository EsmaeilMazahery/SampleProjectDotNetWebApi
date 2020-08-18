
using ESkimo.DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ESkimo.DataLayer.Mappings
{
    public class AreaMapping : IEntityTypeConfiguration<Area>
    {
        public void Configure(EntityTypeBuilder<Area> builder)
        {
            builder.HasMany(m => m.memberLocations)
                .WithOne(o => o.area)
                .HasForeignKey(f => f.areaId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(b => b.location).HasColumnType("geography");
        }
    }
}