namespace TelemetryService.Interfaces.Services;

public interface IApiKeyService
{
    Task<Guid?> ValidateAsync(string apiKey, CancellationToken ct = default);
}