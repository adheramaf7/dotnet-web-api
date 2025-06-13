using Microsoft.Extensions.DependencyInjection;
using WebApi.Infrastructure.Mapping;

namespace WebApi.Infrastructure.DependencyInjection
{
    public static class AutoMapperRegistration
    {
        public static IServiceCollection AddAutoMapperServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile).Assembly);

            return services;
        }
    }
}