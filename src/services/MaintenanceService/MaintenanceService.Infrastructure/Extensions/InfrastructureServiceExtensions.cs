using Azure.AI.OpenAI;
using IoT.Contracts.Events;
using MaintenanceService.Infrastructure.Consumers;
using MaintenanceService.Infrastructure.Options;
using MaintenanceService.Infrastructure.Persistence;
using MaintenanceService.Infrastructure.Repositories;
using MaintenanceService.Infrastructure.Services;
using MaintenanceService.Interfaces;
using MaintenanceService.Interfaces.Repositories;
using MaintenanceService.Interfaces.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace MaintenanceService.Infrastructure.Extensions;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration config)
    {

        services.AddDbContext<MaintenanceDbContext>(o =>
            o.UseNpgsql(config.GetConnectionString("Default"),
                npgsql => npgsql.UseVector()));


        services.Configure<KafkaOptions>(config.GetSection(KafkaOptions.SectionName));


        services.Configure<AzureAIOptions>(config.GetSection(AzureAIOptions.SectionName));


        services.AddSingleton(sp =>
        {
            var opts = sp.GetRequiredService<IOptions<AzureAIOptions>>().Value;
            return new AzureOpenAIClient(new Uri(opts.Endpoint), new System.ClientModel.ApiKeyCredential(opts.ApiKey));
        });


        services.AddScoped<IMaintenanceRecordRepository, MaintenanceRecordRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();


        services.AddScoped<IEmbeddingService, EmbeddingService>();


        services.AddMassTransit(x =>
        {
            x.AddConsumer<EmbeddingGenerationConsumer>();
            x.UsingInMemory((ctx, cfg) => cfg.ConfigureEndpoints(ctx));

            x.AddRider(rider =>
            {
                rider.AddConsumer<EmbeddingGenerationConsumer>();
                rider.AddProducer<MaintenanceRecordCreatedEvent>(
                    config["Kafka:Topics:MaintenanceCreated"]);

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

                    k.TopicEndpoint<MaintenanceRecordCreatedEvent>(
                        opts.Topics.MaintenanceCreated,
                        opts.ConsumerGroups.EmbeddingGenerator,
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

        return services;
    }
}