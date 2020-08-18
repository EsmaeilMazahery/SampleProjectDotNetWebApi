
using ESkimo.DomainLayer.Models;
using ESkimo.Infrastructure.Enumerations;
using ESkimo.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Linq;

namespace ESkimo.DataLayer.Mappings
{
    public class RoleMapping : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            
        }

    }
}