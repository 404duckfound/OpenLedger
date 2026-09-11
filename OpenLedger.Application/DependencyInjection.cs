using FluentValidation;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace OpenLedger.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;

            services.AddValidatorsFromAssembly(assembly);

            services.AddMapster();

            return services;
        }
    }
}
