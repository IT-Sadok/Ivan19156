using Microsoft.Extensions.DependencyInjection;

namespace IoT.Rest.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddOpenApi();
        return services;
    }
}