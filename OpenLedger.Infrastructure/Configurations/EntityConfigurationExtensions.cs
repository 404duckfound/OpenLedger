using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenLedger.Domain.Base;

namespace OpenLedger.Infrastructure.Configurations
{
    public static class EntityConfigurationExtensions
    {
        public static EntityTypeBuilder<T> ConfigureBaseEntity<T>(this EntityTypeBuilder<T> builder) where T : BaseEntity
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id)
                .IsRequired()
                .HasDefaultValueSql("gen_random_uuid()")
                .ValueGeneratedOnAdd();

            builder.Property(c => c.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAdd();

            return builder;
        }

        public static EntityTypeBuilder<T> ConfigureBaseTenantEntity<T>(this EntityTypeBuilder<T> builder) where T : BaseTenantEntity
        {
            builder.ConfigureBaseEntity();

            builder.HasIndex(c => c.TenantId);
            builder.Property(c => c.TenantId)
                .ValueGeneratedNever();

            return builder;
        }
    }
}
