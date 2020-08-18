
using ESkimo.DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ESkimo.DataLayer.Mappings
{
    public class Rel_DiscountCodeBrandMapping : IEntityTypeConfiguration<Rel_DiscountCodeBrand>
    {
        public void Configure(EntityTypeBuilder<Rel_DiscountCodeBrand> builder)
        {
            builder.HasKey(bc => new { bc.discountCodeId, bc.brandId });

            builder.HasOne(bc => bc.discountCode)
                .WithMany(b => b.brands)
                .HasForeignKey(bc => bc.discountCodeId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(bc => bc.brand)
                 .WithMany(c => c.discountCodes)
                 .HasForeignKey(bc => bc.brandId)
                 .IsRequired(true)
                 .OnDelete(DeleteBehavior.Cascade);
        }

    }
}