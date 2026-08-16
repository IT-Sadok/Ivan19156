using TelemetryService.Interfaces.Services;

namespace TelemetryService.Infrastructure.Services;

public class ApiKeyService : IApiKeyService
{
    private readonly IDeviceServiceClient _deviceServiceClient;

    public ApiKeyService(IDeviceServiceClient deviceServiceClient)
        => _deviceServiceClient = deviceServiceClient;

    public Task<Guid?> ValidateAsync(string apiKey, CancellationToken ct = default)
        => _deviceServiceClient.ValidateApiKeyAsync(apiKey, ct);
}
