namespace TelemetryService.Interfaces.Services;

public interface IDeviceServiceClient
{
    Task<Guid?> ValidateApiKeyAsync(string apiKey, CancellationToken ct = default);
}
