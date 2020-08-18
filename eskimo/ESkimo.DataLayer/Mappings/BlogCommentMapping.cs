
using ESkimo.DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ESkimo.DataLayer.Mappings
{
    public class BlogCommentMapping : IEntityTypeConfiguration<BlogComment>
    {
        public void Configure(EntityTypeBuilder<BlogComment> builder)
        {
            builder.HasOne(o => o.member)
                .WithMany(m => m.blogComments)
                .HasForeignKey(f => f.memberId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(o => o.user)
                .WithMany(m => m.blogComments)
                .HasForeignKey(f => f.userId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(o => o.blogPost)
                .WithMany(m => m.blogComments)
                .HasForeignKey(f => f.blogPostId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}