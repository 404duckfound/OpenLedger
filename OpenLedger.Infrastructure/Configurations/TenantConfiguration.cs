using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenLedger.Domain.Entities.Auth;

namespace OpenLedger.Infrastructure.Configurations
{
    public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
    {
        public void Configure(EntityTypeBuilder<Tenant> builder)
        {
            builder.ConfigureBaseEntity();

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(c => c.Email)
                .HasMaxLength(100);

            builder.Property(c => c.TaxNumber)
                .HasMaxLength(50);

            builder.Property(c => c.TaxOffice)
                .HasMaxLength(50);

            builder.Property(c => c.PhoneNumber)
                .HasMaxLength(50);

            builder.Property(c => c.Address)
                .HasMaxLength(100);

            builder.Property(c => c.SubscriptionExpiration)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP + INTERVAL '30 days'")
                .ValueGeneratedOnAdd();

            builder.Ignore(c => c.IsSubscriptionEnd);
        }
    }
}
