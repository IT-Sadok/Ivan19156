using DotNet.Testcontainers.Builders;
using MaintenanceService.IntegrationTests.Infrastructure;
using Testcontainers.Kafka;
using Testcontainers.PostgreSql;

namespace MaintenanceService.IntegrationTests.Fixtures;

[CollectionDefinition("Integration")]
public class IntegrationCollection : ICollectionFixture<IntegrationTestFixture>;

public class IntegrationTestFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder()
        .WithImage("pgvector/pgvector:pg16")
        .WithDatabase("maintenance_test_db")
        .WithUsername("iot_user")
        .WithPassword("iot_password")
        .Build();

    private readonly KafkaContainer _kafka = new KafkaBuilder()
        .Build();

    public string PostgresConnectionString => _postgres.GetConnectionString();
    public string KafkaBootstrapServers => _kafka.GetBootstrapAddress();

    public MaintenanceWebApplicationFactory Factory { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        await Task.WhenAll(_postgres.StartAsync(), _kafka.StartAsync());
        Factory = new MaintenanceWebApplicationFactory(this);
    }

    public async Task DisposeAsync()
    {
        await Factory.DisposeAsync();
        await Task.WhenAll(_postgres.StopAsync(), _kafka.StopAsync());
    }
}