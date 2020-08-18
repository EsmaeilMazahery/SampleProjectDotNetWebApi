
using ESkimo.DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ESkimo.DataLayer.Mappings
{
    public class BlogCategoryMapping : IEntityTypeConfiguration<BlogCategory>
    {
        public void Configure(EntityTypeBuilder<BlogCategory> builder)
        {
            builder.HasMany(m => m.children)
                .WithOne(o => o.parent)
                .HasForeignKey(f => f.parentId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasMany(m => m.blogPosts)
                .WithOne(o => o.blogCategory)
                .HasForeignKey(f => f.blogCategoryId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        }

    }
}