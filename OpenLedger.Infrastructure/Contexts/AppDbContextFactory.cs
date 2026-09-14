using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using OpenLedger.Application.Options;

namespace OpenLedger.Infrastructure.Contexts
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var basePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "../OpenLedger.API"));

            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var dbOptions = configuration.GetSection(DbOptions.SectionName).Get<DbOptions>();

            if (string.IsNullOrEmpty(dbOptions!.DbConnectionString))
            {
                throw new InvalidOperationException($"Connection string 'Base' could not be loaded for environment '{environment}'.");
            }

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseNpgsql(dbOptions.DbConnectionString, b => b.MigrationsAssembly("OpenLedger.Infrastructure"));

            return new AppDbContext(null!, optionsBuilder.Options);
        }
    }
}