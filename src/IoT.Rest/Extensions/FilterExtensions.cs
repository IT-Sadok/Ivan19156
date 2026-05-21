using IoT.Rest.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace IoT.Rest.Extensions;

public static class FilterExtensions
{
    public static IServiceCollection AddFilters(this IServiceCollection services)
    {
        services.AddScoped<ApiKeyAuthFilter>();
        return services;
    }
}