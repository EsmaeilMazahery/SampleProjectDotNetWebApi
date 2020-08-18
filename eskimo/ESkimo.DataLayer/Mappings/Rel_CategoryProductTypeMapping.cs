
using ESkimo.DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ESkimo.DataLayer.Mappings
{
    public class Rel_CategoryProductTypeMapping : IEntityTypeConfiguration<Rel_CategoryProductType>
    {
        public void Configure(EntityTypeBuilder<Rel_CategoryProductType> builder)
        {
            builder.HasKey(bc => new { bc.categoryId, bc.productTypeId });

            builder.HasOne(bc => bc.category)
                .WithMany(b => b.productTypes)
                .HasForeignKey(bc => bc.categoryId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(bc => bc.productType)
                 .WithMany(c => c.categories)
                 .HasForeignKey(bc => bc.productTypeId)
                 .IsRequired(true)
                 .OnDelete(DeleteBehavior.Cascade);
        }

    }
}