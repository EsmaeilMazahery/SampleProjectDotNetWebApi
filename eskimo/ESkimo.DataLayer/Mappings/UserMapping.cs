
using ESkimo.DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ESkimo.DataLayer.Mappings
{
    public class UserMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasIndex(b => b.username)
                .IsUnique()
                .HasName("UI_Username");

            builder.HasIndex(b => b.mobile)
                .IsUnique()
                .HasName("UI_Mobile");

             builder.HasIndex(b => b.email)
                     .IsUnique()
                     .HasName("UI_Email");
        }

    }
}