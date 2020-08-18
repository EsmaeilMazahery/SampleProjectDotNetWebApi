
using SP.DomainLayer.Models;
using SP.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace SP.DataLayer.Mappings
{
    public class UserMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasMany(m => m.smsLogs)
                .WithOne(o => o.user)
                .HasForeignKey(f => f.userId);
        }

    }
}