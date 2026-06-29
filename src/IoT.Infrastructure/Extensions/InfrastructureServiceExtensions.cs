// IoT.Infrastructure/Extensions/InfrastructureServiceExtensions.cs

using Azure;
using Azure.AI.OpenAI;
using IoT.Contracts.Events;
using IoT.Contracts.Kafka;
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
            x.AddConsumer<EmbeddingGenerationConsumer>();

            x.UsingInMemory((context, cfg) =>
            {
                cfg.ConfigureEndpoints(context);
            });

            x.AddRider(rider =>
            {
                // Producers
                rider.AddProducer<TelemetryReceivedEvent>(KafkaTopics.Telemetry);
                rider.AddProducer<MaintenanceRecordCreatedEvent>(KafkaTopics.EmbeddingGeneration);

                // Consumers
                rider.AddConsumer<TelemetryConsumer>();
                rider.AddConsumer<RulesEngineConsumer>();
                rider.AddConsumer<EmbeddingGenerationConsumer>();

                rider.UsingKafka((context, k) =>
                {
                    k.Host(config["Kafka:BootstrapServers"]);

                    k.TopicEndpoint<TelemetryReceivedEvent>(
                        KafkaTopics.Telemetry,
                        KafkaTopics.ConsumerGroups.TelemetryProcessor,
                        e =>
                        {
                            e.CreateIfMissing(t =>
                            {
                                t.NumPartitions = 1;
                                t.ReplicationFactor = 1;
                            });
                            e.ConfigureConsumer<TelemetryConsumer>(context);
                        });

                    k.TopicEndpoint<TelemetryReceivedEvent>(
                        KafkaTopics.Telemetry,
                        KafkaTopics.ConsumerGroups.RulesEngine,
                        e =>
                        {
                            e.CreateIfMissing(t =>
                            {
                                t.NumPartitions = 1;
                                t.ReplicationFactor = 1;
                            });
                            e.ConfigureConsumer<RulesEngineConsumer>(context);
                        });

                    k.TopicEndpoint<MaintenanceRecordCreatedEvent>(
                        KafkaTopics.EmbeddingGeneration,
                        KafkaTopics.ConsumerGroups.EmbeddingGenerator,
                        e =>
                        {
                            e.CreateIfMissing(t =>
                            {
                                t.NumPartitions = 1;
                                t.ReplicationFactor = 1;
                            });
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