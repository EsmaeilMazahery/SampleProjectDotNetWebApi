
using ESkimo.DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ESkimo.DataLayer.Mappings
{
    public class BlogPostMapping : IEntityTypeConfiguration<BlogPost>
    {
        public void Configure(EntityTypeBuilder<BlogPost> builder)
        {
            builder.HasOne(o => o.user)
                .WithMany(m => m.blogPosts)
                .HasForeignKey(f => f.userId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(o => o.blogCategory)
                .WithMany(m => m.blogPosts)
                .HasForeignKey(f => f.blogCategoryId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}