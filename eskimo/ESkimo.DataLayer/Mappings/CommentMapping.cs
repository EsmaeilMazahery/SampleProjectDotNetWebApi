
using ESkimo.DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ESkimo.DataLayer.Mappings
{
    public class CommentMapping : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasOne(o => o.product)
                .WithMany(m => m.comments)
                .HasForeignKey(f => f.productId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(o => o.member)
                .WithMany(m => m.comments)
                .HasForeignKey(f => f.memberId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}