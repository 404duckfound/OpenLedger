using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenLedger.Application.Interfaces.Repositories.Base;
using OpenLedger.Application.Interfaces.Repositories.Customs;
using OpenLedger.Application.Interfaces.Singletons;
using OpenLedger.Infrastructure.Contexts;
using OpenLedger.Infrastructure.Repositories;
using OpenLedger.Infrastructure.Repositories.Base;
using OpenLedger.Infrastructure.Singletons;

namespace OpenLedger.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddSingleton<ITokenGenerator, TokenGenerator>();
            services.AddSingleton<IPasswordHasher, PasswordHasher>();

            services.AddDbContext<AppDbContext>((options) =>
            {
                var connectionString = configuration.GetConnectionString("Base") ?? string.Empty;
                options.UseNpgsql(connectionString, b => b.MigrationsAssembly("OpenLedger.Infrastructure"));
            });

            return services;
        }
    }
}