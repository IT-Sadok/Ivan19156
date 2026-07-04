using IoT.Interfaces.Services;

namespace IoT.IntegrationTests.Infrastructure;

public class MockApiKeyService : IApiKeyService
{
    private readonly Guid _deviceId;

    public MockApiKeyService(Guid deviceId)
        => _deviceId = deviceId;

    public Task<string> GenerateAsync(Guid deviceId, CancellationToken ct = default)
        => Task.FromResult("test-api-key");

    public Task<Guid?> ValidateAsync(string apiKey, CancellationToken ct = default)
        => Task.FromResult<Guid?>(_deviceId);
}