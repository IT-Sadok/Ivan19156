namespace DeviceService.Interfaces.Services;

public interface IApiKeyService
{
    Task<string> GenerateAsync(Guid deviceId, CancellationToken ct = default);
    Task<Guid?> ValidateAsync(string apiKey, CancellationToken ct = default);
    Task<bool> RevokeAsync(Guid apiKeyId, CancellationToken ct = default);
}
