using MaintenanceService.Interfaces.Services;

namespace MaintenanceService.IntegrationTests.Infrastructure;

public class MockEmbeddingService : IEmbeddingService
{
    public Task<float[]> GenerateEmbeddingAsync(string text, CancellationToken ct = default)
        => Task.FromResult(new float[1536]);
}