
using ESkimo.DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ESkimo.DataLayer.Mappings
{
    public class CategoryMapping : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasMany(m => m.children)
                .WithOne(o => o.parent)
                .HasForeignKey(f => f.parentId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(m => m.products)
                .WithOne(o => o.category)
                .HasForeignKey(f => f.categoryId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(m => m.brands)
                .WithOne(o => o.category)
                .HasForeignKey(f => f.categoryId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(m => m.productTypes)
                .WithOne(o => o.category)
                .HasForeignKey(f => f.categoryId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(m => m.discountCodes)
                .WithOne(o => o.category)
                .HasForeignKey(f => f.categoryId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}