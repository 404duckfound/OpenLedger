using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenLedger.Domain.Entities.Auth;
using OpenLedger.Domain.Enums;

namespace OpenLedger.Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ConfigureBaseTenantEntity();

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(c => c.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.PasswordHash)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(c => c.IsVerified)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(c => c.Role)
                .IsRequired()
                .HasDefaultValue(UserRole.User)
                .HasConversion<string>()
                .HasMaxLength(30);
        }
    }
}
