using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OpenLedger.Application.Behaviors;
using OpenLedger.Application.Options;
using System.Reflection;

namespace OpenLedger.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            services.AddValidatorsFromAssembly(assembly);

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(assembly);

                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            });

            services.AddOptionsWithValidateOnStart<DbOptions>("Db");
            services.AddOptionsWithValidateOnStart<TokenOptions>("Token");

            return services;
        }
    }
}
