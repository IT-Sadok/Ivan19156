using Testcontainers.Kafka;
using Testcontainers.PostgreSql;
using Testcontainers.Redis;

namespace IoT.IntegrationTests.Fixtures;

public class IntegrationTestFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder()
        .WithImage("pgvector/pgvector:pg16")
        .WithDatabase("iot_test_db")
        .WithUsername("iot_user")
        .WithPassword("iot_password")
        .Build();

    private readonly KafkaContainer _kafka = new KafkaBuilder()
        .WithImage("confluentinc/cp-kafka:7.6.0")
        .Build();
    
    private readonly RedisContainer _redis = new RedisBuilder()
        .WithImage("redis:7")
        .Build();

    public string PostgresConnectionString => _postgres.GetConnectionString();
    public string KafkaBootstrapServers => _kafka.GetBootstrapAddress();
    public string RedisConnectionString => _redis.GetConnectionString();

    public async Task InitializeAsync()
    {
        await Task.WhenAll(
            _postgres.StartAsync(),
            _kafka.StartAsync(),
            _redis.StartAsync());
    }

    public async Task DisposeAsync()
    {
        await Task.WhenAll(
            _postgres.StopAsync(),
            _kafka.StopAsync(),
            _redis.StopAsync());
    }
}

[CollectionDefinition("Integration")]
public class IntegrationCollection : ICollectionFixture<IntegrationTestFixture>;