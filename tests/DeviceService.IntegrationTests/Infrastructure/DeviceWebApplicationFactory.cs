using DeviceService.Infrastructure.Persistence;
using DeviceService.IntegrationTests.Fixtures;
using DeviceService.Interfaces.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DeviceService.IntegrationTests.Infrastructure;

public class DeviceWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly IntegrationTestFixture _fixture;

    public DeviceWebApplicationFactory(IntegrationTestFixture fixture) => _fixture = fixture;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("ConnectionStrings:Default", _fixture.PostgresConnectionString);
        builder.UseSetting("ConnectionStrings:Redis", "localhost:6379");
        builder.UseSetting("Kafka:BootstrapServers", _fixture.KafkaBootstrapServers);
        builder.UseSetting("Kafka:Topics:TelemetryEvents", "telemetry-stored");
        builder.UseSetting("Kafka:Topics:DeviceEvents", "device-events");
        builder.UseSetting("Kafka:ConsumerGroups:DeviceProcessor", "device-processor");
        builder.UseSetting("Jwt:Secret", TestConstants.Jwt.Secret);
        builder.UseSetting("Jwt:Issuer", TestConstants.Jwt.Issuer);
        builder.UseSetting("Jwt:Audience", TestConstants.Jwt.Audience);
        builder.UseSetting("DeviceService:BaseUrl", "http://localhost/");

        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Secret"] = TestConstants.Jwt.Secret,
                ["Jwt:Issuer"] = TestConstants.Jwt.Issuer,
                ["Jwt:Audience"] = TestConstants.Jwt.Audience,
            });
        });
        
        builder.ConfigureServices(services =>
        {
            ReplaceService<IApiKeyService>(services, _ => new MockApiKeyService(TestConstants.DeviceId));

            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<DeviceDbContext>();
            db.Database.Migrate();
        });
    }

    public Task InitializeAsync() => Task.CompletedTask;
    public new Task DisposeAsync() => base.DisposeAsync().AsTask();

    private static void ReplaceService<TService>(IServiceCollection services, Func<IServiceProvider, TService> factory)
        where TService : class
    {
        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(TService));
        if (descriptor != null) services.Remove(descriptor);
        services.AddScoped<TService>(factory);
    }
}
