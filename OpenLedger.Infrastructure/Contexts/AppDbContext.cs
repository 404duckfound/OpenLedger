using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OpenLedger.Domain.Entities.Auth;
using OpenLedger.Infrastructure.Options;

namespace OpenLedger.Infrastructure.Contexts
{
    public class AppDbContext(IOptions<DbOptions> options) : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseNpgsql(options.Value.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }

        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}