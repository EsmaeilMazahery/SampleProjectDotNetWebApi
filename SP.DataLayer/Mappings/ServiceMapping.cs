
using SP.DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SP.DataLayer.Mappings
{
    public class ServiceMapping : IEntityTypeConfiguration<Service>
    {
        public void Configure(EntityTypeBuilder<Service> builder)
        {
            builder.HasKey(s => s.serviceId);

            //سرویسهای هر کاربر باید یونیک باشد
            builder.HasIndex(b => new { b.name, b.memberId })
                   .IsUnique()
                   .HasName("UI_UserService");

            builder.HasOne(s => s.member)
            .WithMany(u => u.services)
            .HasForeignKey(f => f.memberId);
        }
    }
}