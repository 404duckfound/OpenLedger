using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenLedger.Application.Interfaces.Repositories.Base;
using OpenLedger.Application.Interfaces.Repositories.Customs;
using OpenLedger.Application.Interfaces.Services;
using OpenLedger.Application.Options;
using OpenLedger.Infrastructure.Contexts;
using OpenLedger.Infrastructure.Repositories.Base;
using OpenLedger.Infrastructure.Repositories.Customs;
using OpenLedger.Infrastructure.Services;

namespace OpenLedger.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var dbOptions = configuration.GetSection(DbOptions.SectionName).Get<DbOptions>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<ITenantRepository, TenantRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddTransient<ITokenGeneratorService, TokenGeneratorService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IPasswordHasherService, PasswordHasherService>();

            services.AddDbContext<AppDbContext>((options) =>
            {
                options.UseNpgsql(dbOptions!.DbConnectionString, b => b.MigrationsAssembly("OpenLedger.Infrastructure"));
            });

            return services;
        }
    }
}