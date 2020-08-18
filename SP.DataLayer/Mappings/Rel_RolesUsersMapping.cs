
using SP.DomainLayer.Models;
using SP.Infrastructure.Enumerations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Linq;

namespace SP.DataLayer.Mappings
{
    public class Rel_RolesUsersMapping : IEntityTypeConfiguration<Rel_RolesUsers>
    {
        public void Configure(EntityTypeBuilder<Rel_RolesUsers> builder)
        {
            builder.HasKey(bc => new { bc.userId, bc.roleId });
            
            builder.HasOne(bc => bc.role)
                .WithMany(b => b.rolesUsers)
                .HasForeignKey(bc => bc.roleId);

            builder.HasOne(bc => bc.user)
                 .WithMany(c => c.rolesUsers)
                 .HasForeignKey(bc => bc.userId);
        }
    }
}