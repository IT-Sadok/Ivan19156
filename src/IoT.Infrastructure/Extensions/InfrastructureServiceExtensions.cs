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
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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

        services.AddMassTransit(x =>
{
    x.AddConsumer<RulesEngineConsumer>();
 

    x.UsingInMemory((ctx, cfg) => cfg.ConfigureEndpoints(ctx));

    x.AddRider(rider =>
    {
        var kafkaOptions = config.GetSection(KafkaOptions.SectionName).Get<KafkaOptions>()!;

        rider.AddProducer<TelemetryReceivedEvent>(kafkaOptions.Topics.Telemetry);
        
        rider.AddConsumer<RulesEngineConsumer>();
        

        rider.UsingKafka((context, k) =>
        {
            var opts = context.GetRequiredService<IOptions<KafkaOptions>>().Value;
    
            var bootstrapServers = context.GetService<KafkaBootstrapOverride>()?.BootstrapServers
                                   ?? opts.BootstrapServers;
            var logger = context.GetRequiredService<ILoggerFactory>().CreateLogger("KafkaSetup");
    
            logger.LogInformation("Kafka ConnectionString is null: {IsNull}", string.IsNullOrEmpty(opts.ConnectionString));
            logger.LogInformation("Kafka BootstrapServers: {Servers}", opts.BootstrapServers);
            k.Host(bootstrapServers, h =>
            {
                if (!string.IsNullOrEmpty(opts.ConnectionString))
                {
                    h.UseSasl(sasl =>
                    {
                        sasl.SecurityProtocol = Confluent.Kafka.SecurityProtocol.SaslSsl;
                        sasl.Mechanism = Confluent.Kafka.SaslMechanism.Plain;
                        sasl.Username = "$ConnectionString";
                        sasl.Password = opts.ConnectionString;
                    });
                }
            });

            k.TopicEndpoint<TelemetryReceivedEvent>(
                opts.Topics.Telemetry,
                opts.ConsumerGroups.TelemetryProcessor,
                e =>
                {
                    e.CreateIfMissing(t => { t.NumPartitions = 1; t.ReplicationFactor = 1; });
                });

            k.TopicEndpoint<TelemetryReceivedEvent>(
                opts.Topics.Telemetry,
                opts.ConsumerGroups.RulesEngine,
                e =>
                {
                    e.CreateIfMissing(t => { t.NumPartitions = 1; t.ReplicationFactor = 1; });
                    e.ConfigureConsumer<RulesEngineConsumer>(context);
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
    
    public static void AddKafkaRider(
        this IBusRegistrationConfigurator x,
        string bootstrapServers,
        KafkaOptions kafkaOptions)
    {
        x.AddRider(rider =>
        {
            rider.AddProducer<TelemetryReceivedEvent>(kafkaOptions.Topics.Telemetry);
            
            
            rider.AddConsumer<RulesEngineConsumer>();
            

            rider.UsingKafka((context, k) =>
            {
                var opts = context.GetRequiredService<IOptions<KafkaOptions>>().Value;
    
                var bootstrapServers = context.GetService<KafkaBootstrapOverride>()?.BootstrapServers
                                       ?? opts.BootstrapServers;

                var logger = context.GetRequiredService<ILoggerFactory>().CreateLogger("KafkaSetup");
    
                logger.LogInformation("Kafka ConnectionString is null: {IsNull}", string.IsNullOrEmpty(opts.ConnectionString));
                logger.LogInformation("Kafka BootstrapServers: {Servers}", opts.BootstrapServers);
                k.Host(bootstrapServers, h =>
                {
                    if (!string.IsNullOrEmpty(opts.ConnectionString))
                    {
                        h.UseSasl(sasl =>
                        {
                            sasl.SecurityProtocol = Confluent.Kafka.SecurityProtocol.SaslSsl;
                            sasl.Mechanism = Confluent.Kafka.SaslMechanism.Plain;
                            sasl.Username = "$ConnectionString";
                            sasl.Password = opts.ConnectionString;
                        });
                    }
                });

                k.TopicEndpoint<TelemetryReceivedEvent>(
                    opts.Topics.Telemetry,
                    opts.ConsumerGroups.TelemetryProcessor,
                    e =>
                    {
                        e.CreateIfMissing(t => { t.NumPartitions = 1; t.ReplicationFactor = 1; });
                    });

                k.TopicEndpoint<TelemetryReceivedEvent>(
                    opts.Topics.Telemetry,
                    opts.ConsumerGroups.RulesEngine,
                    e =>
                    {
                        e.CreateIfMissing(t => { t.NumPartitions = 1; t.ReplicationFactor = 1; });
                        e.ConfigureConsumer<RulesEngineConsumer>(context);
                    });
                
            });
        });
    }
}