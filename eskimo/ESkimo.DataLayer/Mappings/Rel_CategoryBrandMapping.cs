
using ESkimo.DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ESkimo.DataLayer.Mappings
{
    public class Rel_CategoryBrandMapping : IEntityTypeConfiguration<Rel_CategoryBrand>
    {
        public void Configure(EntityTypeBuilder<Rel_CategoryBrand> builder)
        {
            builder.HasKey(bc => new { bc.brandId, bc.categoryId });

            builder.HasOne(bc => bc.brand)
                .WithMany(b => b.categories)
                .HasForeignKey(bc => bc.brandId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(bc => bc.category)
                 .WithMany(c => c.brands)
                 .HasForeignKey(bc => bc.categoryId)
                 .IsRequired(true)
                 .OnDelete(DeleteBehavior.Cascade);
        }

    }
}