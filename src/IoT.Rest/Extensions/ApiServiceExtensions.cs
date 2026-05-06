using IoT.Application.Common.Mappings;

namespace IoT.Rest.Extensions;

public static class ApiServiceExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => cfg.AddMaps(typeof(DeviceMappingProfile).Assembly));
        return services;
    }
}
