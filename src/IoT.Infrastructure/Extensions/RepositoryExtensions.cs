// IoT.Infrastructure/Extensions/RepositoryExtensions.cs
using IoT.Infrastructure.Persistence;
using IoT.Infrastructure.Repositories;
using IoT.Infrastructure.Services;
using IoT.Interfaces;
using IoT.Interfaces.Repositories;
using IoT.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace IoT.Infrastructure.Extensions;

public static class RepositoryExtensions
{
    public static IServiceCollection AddRepositories(
        this IServiceCollection services)
    {
        services.AddScoped<IDeviceRepository, DeviceRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IDeviceCommandRepository, DeviceCommandRepository>();
        services.AddScoped<ICommandTypeRepository, CommandTypeRepository>();
        services.AddScoped<ITelemetryRepository, TelemetryRepository>();
        
        services.AddScoped<ICacheService, CacheService>();
        services.AddScoped<IDeviceApiKeyRepository, DeviceApiKeyRepository>();
        services.AddScoped<IApiKeyService, ApiKeyService>();
        return services;
    }
}