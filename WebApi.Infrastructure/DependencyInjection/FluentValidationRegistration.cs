using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Application.Common;
using WebApi.Application.Validators;
using WebApi.Application.Validators.Auth;

namespace WebApi.Infrastructure.DependencyInjection
{
    public static class FluentValidationRegistration
    {
        public static IServiceCollection AddFluentValidationServices(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                        .Where(x => x.Value?.Errors.Count > 0)
                        .SelectMany(kvp => kvp.Value!.Errors.Select(e => new
                        {
                            Field = kvp.Key,
                            Error = e.ErrorMessage
                        }))
                        .ToList();

                    var errorResponse = ApiResponse<object>.FailResponse(
                        "Validation errors",
                        errors: [.. errors.Select(e => e.Error.Replace("\'", ""))]
                    );

                    return new BadRequestObjectResult(errorResponse);
                };
            });

            services.AddFluentValidationAutoValidation();

            services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<LoginRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<RefreshTokenRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<ChangePasswordRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdateProfileRequestValidator>();

            services.AddValidatorsFromAssemblyContaining<SaveContactRequestValidator>();


            return services;
        }
    }
}