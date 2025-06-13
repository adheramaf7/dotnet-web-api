using Microsoft.Extensions.DependencyInjection;
using WebApi.Application.Interfaces.Service;
using WebApi.Infrastructure.Services;

namespace WebApi.Infrastructure.DependencyInjection
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<JwtTokenGenerator>();
            services.AddScoped<IAuthService, AuthService>();

            return services;
        }
    }
}