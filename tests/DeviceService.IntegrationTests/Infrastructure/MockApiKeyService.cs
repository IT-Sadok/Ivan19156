using DeviceService.Interfaces.Services;

namespace DeviceService.IntegrationTests.Infrastructure;

public class MockApiKeyService : IApiKeyService
{
    private readonly Guid _deviceId;

    public MockApiKeyService(Guid deviceId) => _deviceId = deviceId;

    public Task<string> GenerateAsync(Guid deviceId, CancellationToken ct = default)
        => Task.FromResult(TestConstants.ApiKey);

    public Task<Guid?> ValidateAsync(string apiKey, CancellationToken ct = default)
        => Task.FromResult(apiKey == TestConstants.ApiKey ? (Guid?)_deviceId : null);

    public Task<bool> RevokeAsync(Guid apiKeyId, CancellationToken ct = default)
        => Task.FromResult(true);
}
