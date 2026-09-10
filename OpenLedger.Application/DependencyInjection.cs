using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OpenLedger.Application.Behaviors;
using OpenLedger.Application.Profiles;
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

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(assembly);

                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            });

            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile(typeof(TenantProfile));
            });

            return services;
        }
    }
}
