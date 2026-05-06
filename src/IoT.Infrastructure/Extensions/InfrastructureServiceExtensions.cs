// IoT.Infrastructure/Extensions/InfrastructureServiceExtensions.cs
using IoT.Infrastructure.Persistence;
using IoT.Infrastructure.Services;
using IoT.Interfaces.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IoT.Infrastructure.Consumers;

namespace IoT.Infrastructure.Extensions;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(config.GetConnectionString("Default")));
        
        services.AddMassTransit(x =>
        {
            x.AddConsumer<TelemetryConsumer>();

            x.UsingInMemory((context, cfg) =>
            {
                cfg.ConfigureEndpoints(context);
            });
        });
        services.AddRepositories();
        services.AddScoped<ICommandNotificationService, CommandNotificationService>();

        return services;
    }
}