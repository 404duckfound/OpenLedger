using Microsoft.EntityFrameworkCore;
using OpenLedger.Domain.Entities.Auth;

namespace OpenLedger.Infrastructure.Contexts
{
    public class AppDbContext : DbContext
    {
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}