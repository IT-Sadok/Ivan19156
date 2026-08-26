using IoT.Contracts.Events;
using DeviceService.Infrastructure.Consumers;
using DeviceService.Infrastructure.Options;
using DeviceService.Infrastructure.Persistence;
using DeviceService.Infrastructure.Repositories;
using DeviceService.Infrastructure.Services;
using DeviceService.Interfaces;
using DeviceService.Interfaces.Repositories;
using DeviceService.Interfaces.Services;
using IoT.Contracts.Telemetry;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DeviceService.Infrastructure.Extensions;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration config)
    {
        // EF Core
        services.AddDbContext<DeviceDbContext>(o =>
            o.UseNpgsql(config.GetConnectionString("Default")));

        // Kafka options
        services.Configure<KafkaOptions>(config.GetSection(KafkaOptions.SectionName));

        // Repositories
        services.AddScoped<IDeviceRepository, DeviceRepository>();
        services.AddScoped<IDeviceApiKeyRepository, DeviceApiKeyRepository>();
        services.AddScoped<IDeviceCommandRepository, DeviceCommandRepository>();
        services.AddScoped<ICommandTypeRepository, CommandTypeRepository>();
        services.AddScoped<IAlertRepository, AlertRepository>();
        services.AddScoped<IRuleRepository, RuleRepository>();
        services.AddScoped<IManufacturerRepository, ManufacturerRepository>();
        services.AddScoped<IWarehouseRepository, WarehouseRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Services
        services.AddScoped<IApiKeyService, ApiKeyService>();
        services.AddScoped<ICacheService, CacheService>();
        services.AddScoped<IRulesEngineService, RulesEngineService>();

        // Redis
        services.AddStackExchangeRedisCache(o =>
            o.Configuration = config.GetConnectionString("Redis"));

        // MassTransit + Kafka
        services.AddMassTransit(x =>
        {
            x.AddConsumer<TelemetryStoredEventConsumer>();
            x.UsingInMemory((ctx, cfg) => cfg.ConfigureEndpoints(ctx));

            x.AddRider(rider =>
            {
                rider.AddConsumer<TelemetryStoredEventConsumer>();
                rider.AddProducer<ApiKeyGeneratedEvent>(config["Kafka:Topics:DeviceEvents"]);

                rider.UsingKafka((context, k) =>
                {
                    var opts = context.GetRequiredService<IOptions<KafkaOptions>>().Value;

                    k.Host(opts.BootstrapServers, h =>
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

                    k.TopicEndpoint<TelemetryStoredEvent>(
                        opts.Topics.TelemetryEvents,
                        opts.ConsumerGroups.DeviceProcessor,
                        e =>
                        {
                            e.CreateIfMissing(t =>
                            {
                                t.NumPartitions = 1;
                                t.ReplicationFactor = 1;
                            });
                            e.ConfigureConsumer<TelemetryStoredEventConsumer>(context);
                        });
                });
            });
        });

        return services;
    }
}
