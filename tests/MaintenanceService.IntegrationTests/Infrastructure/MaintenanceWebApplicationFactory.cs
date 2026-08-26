using MaintenanceService.Infrastructure.Persistence;
using MaintenanceService.IntegrationTests.Fixtures;
using MaintenanceService.Interfaces.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MaintenanceService.IntegrationTests.Infrastructure;

public class MaintenanceWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly IntegrationTestFixture _fixture;

    public MaintenanceWebApplicationFactory(IntegrationTestFixture fixture) => _fixture = fixture;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("ConnectionStrings:Default", _fixture.PostgresConnectionString);
        builder.UseSetting("Kafka:BootstrapServers", _fixture.KafkaBootstrapServers);
        builder.UseSetting("Kafka:Topics:MaintenanceCreated", "maintenance-created");
        builder.UseSetting("Kafka:ConsumerGroups:EmbeddingGenerator", "maintenance-embedding-generator");
        builder.UseSetting("Jwt:Key", TestConstants.Jwt.Secret);
        builder.UseSetting("Jwt:Issuer", TestConstants.Jwt.Issuer);
        builder.UseSetting("Jwt:Audience", TestConstants.Jwt.Audience);
        builder.UseSetting("AzureAI:Endpoint", "https://mock.openai.azure.com/");
        builder.UseSetting("AzureAI:ApiKey", "mock-key");
        builder.UseSetting("AzureAI:EmbeddingDeploymentName", "mock-embedding");

        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Key"] = TestConstants.Jwt.Secret,
                ["Jwt:Issuer"] = TestConstants.Jwt.Issuer,
                ["Jwt:Audience"] = TestConstants.Jwt.Audience,
            });
        });

        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IEmbeddingService));
            if (descriptor != null) services.Remove(descriptor);
            services.AddScoped<IEmbeddingService, MockEmbeddingService>();

            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<MaintenanceDbContext>();
            db.Database.ExecuteSqlRaw("CREATE EXTENSION IF NOT EXISTS vector;");
            db.Database.Migrate();
        });
    }

    public Task InitializeAsync() => Task.CompletedTask;
    public new Task DisposeAsync() => base.DisposeAsync().AsTask();
}