using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenLedger.Domain.Entities.Auth;

namespace OpenLedger.Infrastructure.Configurations
{
    public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
    {
        public void Configure(EntityTypeBuilder<Tenant> builder)
        {
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("varchar(100)");
        }
    }
}
