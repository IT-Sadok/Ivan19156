using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TelemetryService.Infrastructure.Consumers;
using TelemetryService.Infrastructure.Options;
using TelemetryService.Infrastructure.Persistence;
using TelemetryService.Infrastructure.Services;
using TelemetryService.Infrastructure.Repositories;
using TelemetryService.Interfaces;
using TelemetryService.Interfaces.Repositories;
using TelemetryService.Interfaces.Services;
using TelemetryService.Domain.Events;
using IoT.Contracts.Telemetry;

namespace TelemetryService.Infrastructure.Extensions;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration config)
    {
        services.AddDbContext<TelemetryDbContext>(options =>
            options.UseNpgsql(config.GetConnectionString("Default")));

        services.Configure<KafkaOptions>(config.GetSection(KafkaOptions.SectionName));

        services.AddScoped<ITelemetryRepository, TelemetryRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddHttpClient<IDeviceServiceClient, DeviceServiceClient>(client =>
        {
            client.BaseAddress = new Uri(config["DeviceService:BaseUrl"]
                ?? throw new InvalidOperationException("DeviceService:BaseUrl is required"));
        });
        services.AddScoped<IApiKeyService, ApiKeyService>();

        services.AddMassTransit(x =>
        {
            x.AddConsumer<TelemetryConsumer>();
            x.UsingInMemory((ctx, cfg) => cfg.ConfigureEndpoints(ctx));

            x.AddRider(rider =>
            {
                rider.AddConsumer<TelemetryConsumer>();
                rider.AddProducer<TelemetryReceivedEvent>(
                    config["Kafka:Topics:Telemetry"]);
                rider.AddProducer<TelemetryStoredEvent>(
                    config["Kafka:Topics:TelemetryStored"]);

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

                    k.TopicEndpoint<TelemetryReceivedEvent>(
                        opts.Topics.Telemetry,
                        opts.ConsumerGroups.TelemetryProcessor,
                        e =>
                        {
                            e.CreateIfMissing(t =>
                            {
                                t.NumPartitions = 1;
                                t.ReplicationFactor = 1;
                            });
                            e.ConfigureConsumer<TelemetryConsumer>(context);
                        });
                });
            });
        });

        return services;
    }
}