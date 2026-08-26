namespace MaintenanceService.Infrastructure.Options;

public class KafkaOptions
{
    public const string SectionName = "Kafka";
    public string BootstrapServers { get; init; } = string.Empty;
    public string? ConnectionString { get; init; }
    public KafkaTopicOptions Topics { get; init; } = new();
    public KafkaConsumerGroupOptions ConsumerGroups { get; init; } = new();
}

public class KafkaTopicOptions
{
    public string MaintenanceCreated { get; init; } = string.Empty;
}

public class KafkaConsumerGroupOptions
{
    public string EmbeddingGenerator { get; init; } = string.Empty;
}