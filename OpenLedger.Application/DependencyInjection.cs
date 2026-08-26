using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using OpenLedger.Application.Interfaces.Services;
using OpenLedger.Application.Options;
using OpenLedger.Application.Services;
using System.Reflection;

namespace OpenLedger.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            services.AddValidatorsFromAssembly(assembly);

            services.AddOptionsWithValidateOnStart<DbOptions>("Db");
            services.AddOptionsWithValidateOnStart<TokenOptions>("Token");

            services.AddScoped<IAuthService, AuthService>();

            return services;
        }
    }
}
