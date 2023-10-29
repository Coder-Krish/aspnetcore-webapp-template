using Application.Common.Interfaces;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services)
        {
            // services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<IAuthenticationService, AuthenticationService>();
            return services;
        }
    }
}
