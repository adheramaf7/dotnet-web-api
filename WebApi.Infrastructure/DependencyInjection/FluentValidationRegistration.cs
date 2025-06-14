using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Application.Validators.Auth;

namespace WebApi.Infrastructure.DependencyInjection
{
    public static class FluentValidationRegistration
    {
        public static IServiceCollection AddFluentValidationServices(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<LoginRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<RefreshTokenRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<ChangePasswordRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdateProfileRequestValidator>();

            services.AddFluentValidationAutoValidation();

            return services;
        }
    }
}