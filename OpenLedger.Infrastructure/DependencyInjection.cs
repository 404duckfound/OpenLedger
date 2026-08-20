using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenLedger.Application.Interfaces.Auth;
using OpenLedger.Application.Interfaces.Repository.Customs;
using OpenLedger.Infrastructure.Contexts;
using OpenLedger.Infrastructure.Options;
using OpenLedger.Infrastructure.Repositories;
using OpenLedger.Infrastructure.Services;

namespace OpenLedger.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptionsWithValidateOnStart<DbOptions>("Db");
            services.AddOptionsWithValidateOnStart<JwtOptions>("Jwt");

            services.AddDbContext<AppDbContext>();

            services.AddScoped<IUserRepository, UserRepository>();

            services.AddSingleton<ITokenGenerator, TokenGenerator>();

            return services;
        }
    }
}