using IoT.Interfaces.Services;

namespace IoT.IntegrationTests.Infrastructure;

public class MockEmbeddingService : IEmbeddingService
{
    public Task<float[]> GenerateEmbeddingAsync(string text, CancellationToken ct = default)
    {
        var embedding = new float[1536];
        for (var i = 0; i < embedding.Length; i++)
            embedding[i] = 0.1f;
        return Task.FromResult(embedding);
    }
}