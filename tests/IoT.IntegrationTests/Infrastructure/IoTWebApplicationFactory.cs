using IoT.Domain.Entities;
using IoT.Domain.Enums;
using IoT.Infrastructure;
using IoT.Infrastructure.Extensions;
using IoT.Infrastructure.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using IoT.IntegrationTests.Fixtures;
using IoT.Interfaces.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IoT.IntegrationTests.Infrastructure;

public class IoTWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly IntegrationTestFixture _fixture;

    public IoTWebApplicationFactory(IntegrationTestFixture fixture)
        => _fixture = fixture;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.Sources.Clear();
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:Default"] = _fixture.PostgresConnectionString,
                ["Kafka:BootstrapServers"] = _fixture.KafkaBootstrapServers,
                ["ConnectionStrings:Redis"] = _fixture.RedisConnectionString,
                ["Jwt:Secret"] = TestConstants.Jwt.Secret,
                ["Jwt:Issuer"] = TestConstants.Jwt.Issuer,
                ["Jwt:Audience"] = TestConstants.Jwt.Audience
            });
        });

        builder.ConfigureServices(services =>
        {
            services.AddSingleton(new KafkaBootstrapOverride(_fixture.KafkaBootstrapServers));

            ReplaceService<IApiKeyService>(services, _ => new MockApiKeyService(TestConstants.DeviceId));
            ReplaceService<IEmbeddingService>(services, _ => new MockEmbeddingService());

            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.Migrate();
            TestDataSeeder.Seed(db);
        });
    }

    public Task InitializeAsync() => Task.CompletedTask;
    public new Task DisposeAsync() => base.DisposeAsync().AsTask();

    private static void ReplaceService<TService>(
        IServiceCollection services,
        Func<IServiceProvider, TService> factory) where TService : class
    {
        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(TService));
        if (descriptor != null)
            services.Remove(descriptor);
        services.AddScoped<TService>(factory);
    }

    private static readonly DateTime SeedTime = new(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    
}