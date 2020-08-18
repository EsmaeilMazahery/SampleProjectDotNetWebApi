
using ESkimo.DomainLayer.Models;
using ESkimo.Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ESkimo.DataLayer.Mappings
{
    public class AreaPriceMapping : IEntityTypeConfiguration<AreaPrice>
    {
        public void Configure(EntityTypeBuilder<AreaPrice> builder)
        {
            builder.HasKey(k => new { k.areaId, k.periodTypeId });

            builder.HasOne(o => o.periodType)
                .WithMany(m => m.areas)
                .HasForeignKey(f => f.periodTypeId);

            builder.HasOne(o => o.area)
                .WithMany(m => m.prices)
                .HasForeignKey(f => f.areaId);

            builder.Property(p => p.amountSend)
                .HasColumnType(ConstantValidations.amountTypeTable);
        }
    }
}