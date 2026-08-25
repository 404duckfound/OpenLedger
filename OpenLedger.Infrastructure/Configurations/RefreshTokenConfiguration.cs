using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenLedger.Domain.Entities.Auth;

namespace OpenLedger.Infrastructure.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ConfigureBaseEntity();

            builder.HasIndex(c => c.UserId);
            builder.Property(c => c.UserId)
                .IsRequired()
                .ValueGeneratedNever();

            builder.HasIndex(c => c.Token)
                .IsUnique();
            builder.Property(c => c.Token)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.UserAgent)
                .IsRequired();

            builder.Property(c => c.ExpiresAt)
                .IsRequired()
                .ValueGeneratedNever();

            builder.Property(c => c.CreatedByIp)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(c => c.RevokedByIp)
                .HasMaxLength(50);

            builder.Property(c => c.RevokedAt)
                .ValueGeneratedNever();

            builder.Property(c => c.ReplacedByToken)
                .HasMaxLength(100);

            builder.Property(c => c.RevokeReason)
                .HasMaxLength(100);

            builder.Ignore(c => c.IsRevoked);
            builder.Ignore(c => c.IsExpired);
            builder.Ignore(c => c.IsActive);
        }
    }
}
