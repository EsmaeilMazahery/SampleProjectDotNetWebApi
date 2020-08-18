
using SP.DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SP.DataLayer.Mappings
{
    public class PropertiseMapping : IEntityTypeConfiguration<Propertise>
    {
        public void Configure(EntityTypeBuilder<Propertise> builder)
        {
            builder.HasMany(p => p.propertiseOptions)
            .WithOne(po => po.propertise)
            .HasForeignKey(po=>po.propertiseKey);
        }

    }
}