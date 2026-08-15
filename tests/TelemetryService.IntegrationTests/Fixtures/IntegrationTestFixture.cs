using TelemetryService.IntegrationTests.Infrastructure;
using Testcontainers.Kafka;
using Testcontainers.PostgreSql;

namespace TelemetryService.IntegrationTests.Fixtures;

public class IntegrationTestFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder()
        .WithImage("pgvector/pgvector:pg16")
        .WithDatabase("telemetry_test_db")
        .WithUsername("iot_user")
        .WithPassword("iot_password")
        .Build();

    private readonly KafkaContainer _kafka = new KafkaBuilder()
        .WithImage("confluentinc/cp-kafka:7.6.0")
        .Build();

    public string PostgresConnectionString => _postgres.GetConnectionString();
    public string KafkaBootstrapServers => _kafka.GetBootstrapAddress();

    public TelemetryWebApplicationFactory Factory { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        await Task.WhenAll(
            _postgres.StartAsync(),
            _kafka.StartAsync());

        Factory = new TelemetryWebApplicationFactory(this);
    }

    public async Task DisposeAsync()
    {
        await Factory.DisposeAsync();
        await Task.WhenAll(
            _postgres.StopAsync(),
            _kafka.StopAsync());
    }
}

[CollectionDefinition("Integration")]
public class IntegrationCollection : ICollectionFixture<IntegrationTestFixture>;