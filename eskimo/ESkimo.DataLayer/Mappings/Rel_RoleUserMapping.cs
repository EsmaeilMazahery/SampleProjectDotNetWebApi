
using ESkimo.DomainLayer.Models;
using ESkimo.Infrastructure.Enumerations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Linq;

namespace ESkimo.DataLayer.Mappings
{
    public class Rel_RoleUserMapping : IEntityTypeConfiguration<Rel_RoleUser>
    {
        public void Configure(EntityTypeBuilder<Rel_RoleUser> builder)
        {
            builder.HasKey(bc => new { bc.userId, bc.roleId });

            builder.HasOne(bc => bc.role)
                .WithMany(b => b.users)
                .HasForeignKey(bc => bc.roleId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(bc => bc.user)
                 .WithMany(c => c.roles)
                 .HasForeignKey(bc => bc.userId)
                 .IsRequired(true)
                 .OnDelete(DeleteBehavior.Cascade);
        }
    }
}