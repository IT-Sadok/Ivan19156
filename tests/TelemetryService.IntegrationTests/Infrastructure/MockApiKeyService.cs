using TelemetryService.Interfaces.Services;

namespace TelemetryService.IntegrationTests.Infrastructure;

public class MockApiKeyService : IApiKeyService
{
    private readonly Guid _deviceId;

    public MockApiKeyService(Guid deviceId)
        => _deviceId = deviceId;

    public Task<Guid?> ValidateAsync(string apiKey, CancellationToken ct = default)
        => Task.FromResult<Guid?>(_deviceId);
}