using Microsoft.Extensions.DependencyInjection;
using OpenLedger.Application.Interfaces.Singletons;
using OpenLedger.Application.Interfaces.Repositories.Customs;
using OpenLedger.Application.Interfaces.Services;
using OpenLedger.Infrastructure.Contexts;
using OpenLedger.Infrastructure.Options;
using OpenLedger.Infrastructure.Repositories;
using OpenLedger.Infrastructure.Services;
using OpenLedger.Infrastructure.Singletons;

namespace OpenLedger.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddOptionsWithValidateOnStart<DbOptions>("Db");
            services.AddOptionsWithValidateOnStart<TokenOptions>("Token");

            services.AddDbContext<AppDbContext>();

            services.AddScoped<IAuthService, AuthService>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

            services.AddSingleton<ITokenGenerator, TokenGenerator>();
            services.AddSingleton<IPasswordHasher, PasswordHasher>();

            return services;
        }
    }
}