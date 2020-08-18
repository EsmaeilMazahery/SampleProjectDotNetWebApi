
using ESkimo.DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ESkimo.DataLayer.Mappings
{
    public class Rel_DiscountCodeCategoryMapping : IEntityTypeConfiguration<Rel_DiscountCodeCategory>
    {
        public void Configure(EntityTypeBuilder<Rel_DiscountCodeCategory> builder)
        {
            builder.HasKey(bc => new { bc.discountCodeId, bc.categoryId });

            builder.HasOne(bc => bc.discountCode)
                .WithMany(b => b.categories)
                .HasForeignKey(bc => bc.discountCodeId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(bc => bc.category)
                 .WithMany(c => c.discountCodes)
                 .HasForeignKey(bc => bc.categoryId)
                 .IsRequired(true)
                 .OnDelete(DeleteBehavior.Cascade);
        }

    }
}