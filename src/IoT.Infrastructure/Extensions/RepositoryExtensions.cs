// IoT.Infrastructure/Extensions/RepositoryExtensions.cs
using IoT.Infrastructure.Persistence;
using IoT.Infrastructure.Repositories;
using IoT.Interfaces;
using IoT.Interfaces.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace IoT.Infrastructure.Extensions;

public static class RepositoryExtensions
{
    public static IServiceCollection AddRepositories(
        this IServiceCollection services)
    {
        services.AddScoped<IDeviceRepository, DeviceRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }
}