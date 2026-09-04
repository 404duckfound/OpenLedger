using Microsoft.EntityFrameworkCore;
using OpenLedger.Application.Interfaces.Services;
using OpenLedger.Domain.Base;
using OpenLedger.Domain.Entities.Auth;

namespace OpenLedger.Infrastructure.Contexts
{
    public class AppDbContext(ICurrentUserService userService, DbContextOptions<AppDbContext> optionsBuilder) : DbContext(optionsBuilder)
    {
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseTenantEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var method = typeof(AppDbContext).GetMethod(nameof(ConfigureTenantFilter), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!.MakeGenericMethod(entityType.ClrType);
                    method.Invoke(this, [modelBuilder]);
                }
            }
        }
        private void ConfigureTenantFilter<T>(ModelBuilder modelBuilder) where T : BaseTenantEntity
        {
            modelBuilder.Entity<T>().HasQueryFilter(q => q.TenantId == userService.TenantId);
        }
    }
}