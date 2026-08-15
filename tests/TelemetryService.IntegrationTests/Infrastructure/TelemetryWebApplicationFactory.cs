using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TelemetryService.Infrastructure.Persistence;
using TelemetryService.IntegrationTests.Fixtures;
using TelemetryService.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace TelemetryService.IntegrationTests.Infrastructure;

public class TelemetryWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly IntegrationTestFixture _fixture;

    public TelemetryWebApplicationFactory(IntegrationTestFixture fixture)
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
                ["Kafka:Topics:Telemetry"] = "telemetry-received",
                ["Kafka:Topics:TelemetryStored"] = "telemetry-stored",
                ["Kafka:ConsumerGroups:TelemetryProcessor"] = "telemetry-processor"
            });
        });

        builder.ConfigureServices(services =>
        {
            ReplaceService<IApiKeyService>(services,
                _ => new MockApiKeyService(TestConstants.DeviceId));

            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<TelemetryDbContext>();
            db.Database.Migrate();
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
}