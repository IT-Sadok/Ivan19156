// IoT.Infrastructure/Extensions/InfrastructureServiceExtensions.cs

using Azure;
using Azure.AI.OpenAI;
using IoT.Contracts.Events;
using IoT.Contracts;
using IoT.Domain.Events;
using IoT.Infrastructure.AI.Functions;
using IoT.Infrastructure.Persistence;
using IoT.Infrastructure.Services;
using IoT.Interfaces.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IoT.Infrastructure.Consumers;
using IoT.Infrastructure.Options;
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
        
        services.Configure<KafkaOptions>(config.GetSection(KafkaOptions.SectionName));
        services.Configure<KafkaOptions>(config.GetSection(KafkaOptions.SectionName));

services.AddMassTransit(x =>
{
    x.AddConsumer<TelemetryConsumer>();
    x.AddConsumer<RulesEngineConsumer>();
    x.AddConsumer<EmbeddingGenerationConsumer>();

    x.UsingInMemory((context, cfg) =>
    {
        cfg.ConfigureEndpoints(context);
    });

    x.AddRider(rider =>
    {
        var kafkaOptions = config.GetSection(KafkaOptions.SectionName).Get<KafkaOptions>()!;

        rider.AddProducer<TelemetryReceivedEvent>(kafkaOptions.Topics.Telemetry);
        rider.AddProducer<MaintenanceRecordCreatedEvent>(kafkaOptions.Topics.EmbeddingGeneration);

        rider.AddConsumer<TelemetryConsumer>();
        rider.AddConsumer<RulesEngineConsumer>();
        rider.AddConsumer<EmbeddingGenerationConsumer>();

        rider.UsingKafka((context, k) =>
        {
            k.Host(kafkaOptions.BootstrapServers);

            k.TopicEndpoint<TelemetryReceivedEvent>(
                kafkaOptions.Topics.Telemetry,
                kafkaOptions.ConsumerGroups.TelemetryProcessor,
                e =>
                {
                    e.CreateIfMissing(t => { t.NumPartitions = 1; t.ReplicationFactor = 1; });
                    e.ConfigureConsumer<TelemetryConsumer>(context);
                });

            k.TopicEndpoint<TelemetryReceivedEvent>(
                kafkaOptions.Topics.Telemetry,
                kafkaOptions.ConsumerGroups.RulesEngine,
                e =>
                {
                    e.CreateIfMissing(t => { t.NumPartitions = 1; t.ReplicationFactor = 1; });
                    e.ConfigureConsumer<RulesEngineConsumer>(context);
                });

            k.TopicEndpoint<MaintenanceRecordCreatedEvent>(
                kafkaOptions.Topics.EmbeddingGeneration,
                kafkaOptions.ConsumerGroups.EmbeddingGenerator,
                e =>
                {
                    e.CreateIfMissing(t => { t.NumPartitions = 1; t.ReplicationFactor = 1; });
                    e.ConfigureConsumer<EmbeddingGenerationConsumer>(context);
                });
        });
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