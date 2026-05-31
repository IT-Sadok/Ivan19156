// IoT.Infrastructure/Extensions/InfrastructureServiceExtensions.cs

using Azure;
using Azure.AI.OpenAI;
using IoT.Infrastructure.AI.Functions;
using IoT.Infrastructure.Persistence;
using IoT.Infrastructure.Services;
using IoT.Interfaces.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IoT.Infrastructure.Consumers;
using Microsoft.Extensions.Options;

namespace IoT.Infrastructure.Extensions;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(config.GetConnectionString("Default")));
        services.Configure<AzureAIOptions>(config.GetSection(AzureAIOptions.SectionName));
        
        services.AddMassTransit(x =>
        {
            x.AddConsumer<TelemetryConsumer>();
            x.AddConsumer<RulesEngineConsumer>();
            
            x.UsingInMemory((context, cfg) =>
            {
                cfg.ConfigureEndpoints(context);
            });
        });
        services.AddRepositories();
        services.AddStackExchangeRedisCache(options =>
            options.Configuration = config.GetConnectionString("Redis"));
        services.AddScoped<ICommandNotificationService, CommandNotificationService>();
        services.AddSingleton(TimeProvider.System);
        services.AddScoped<IAIAssistantService, AzureAIAssistantService>();
        services.AddScoped<IoTContextBuilder>();
        services.AddSingleton(sp =>
        {
            var options = sp.GetRequiredService<IOptions<AzureAIOptions>>().Value;
            return new AzureOpenAIClient(
                new Uri(options.Endpoint),
                new AzureKeyCredential(options.ApiKey));
        });
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(config.GetConnectionString("Default"),
                o => o.UseVector()));

        services.AddScoped<IAIFunction, GetOfflineDevicesFunction>();
        services.AddScoped<IAIFunction, GetActiveAlertsFunction>();
        services.AddScoped<IAIFunction, GetDeviceTelemetryFunction>();
        services.AddScoped<IAIFunction, GetDeviceCommandsFunction>();
        services.AddScoped<IAIFunction, GetSystemSummaryFunction>();
        services.AddScoped<IAIFunction, SearchMaintenanceNotesFunction>();
        
        services.AddScoped<IEmbeddingService, EmbeddingService>();
        
        return services;
    }
}