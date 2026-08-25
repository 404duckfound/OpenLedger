using Microsoft.Extensions.DependencyInjection;
using OpenLedger.Application.Interfaces.Repositories.Base;
using OpenLedger.Application.Interfaces.Repositories.Customs;
using OpenLedger.Application.Interfaces.Services;
using OpenLedger.Application.Interfaces.Singletons;
using OpenLedger.Application.Options;
using OpenLedger.Application.Services;
using OpenLedger.Infrastructure.Repositories;
using OpenLedger.Infrastructure.Repositories.Base;
using OpenLedger.Infrastructure.Singletons;

namespace OpenLedger.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddOptionsWithValidateOnStart<DbOptions>("Db");
            services.AddOptionsWithValidateOnStart<TokenOptions>("Token");

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IAuthService, AuthService>();

            services.AddSingleton<ITokenGenerator, TokenGenerator>();
            services.AddSingleton<IPasswordHasher, PasswordHasher>();

            return services;
        }
    }
}