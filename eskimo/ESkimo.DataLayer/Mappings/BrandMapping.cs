
using ESkimo.DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ESkimo.DataLayer.Mappings
{
    public class BrandMapping : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder.HasMany(m => m.products)
                .WithOne(o => o.brand)
                .HasForeignKey(f => f.brandId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(m => m.categories)
                .WithOne(o => o.brand)
                .HasForeignKey(f => f.brandId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(m => m.discountCodes)
                .WithOne(o => o.brand)
                .HasForeignKey(f => f.brandId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}