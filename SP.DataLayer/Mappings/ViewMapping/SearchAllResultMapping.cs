using SP.DomainLayer.DbViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace SP.DataLayer.Mappings.ViewMapping
{
    public class SearchAllResultConfiguration : IEntityTypeConfiguration<SearchAllResult>
    {
        public void Configure(EntityTypeBuilder<SearchAllResult> builder)
        {
            builder.HasOne(o => o.user).WithMany().OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(o => o.service).WithMany().OnDelete(DeleteBehavior.NoAction);

            builder.Property(b => b.score).HasColumnType("decimal(18, 2)");
        }
    }
}
