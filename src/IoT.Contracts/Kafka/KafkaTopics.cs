namespace IoT.Contracts.Kafka;

public static class KafkaTopics
{
    public const string Telemetry = "iot.telemetry";
    public const string EmbeddingGeneration = "iot.embedding-generation";
    
    public static class ConsumerGroups
        {
            public const string TelemetryProcessor = "iot-telemetry-processor";
            public const string RulesEngine = "iot-rules-engine";
            public const string EmbeddingGenerator = "iot-embedding-generator";
        }
}