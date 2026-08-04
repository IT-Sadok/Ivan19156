using System.Security.Cryptography;
using System.Text;
using TelemetryService.Interfaces.Repositories;
using TelemetryService.Interfaces.Services;

namespace TelemetryService.Infrastructure.Services;

public class ApiKeyService : IApiKeyService
{
    // TODO: Remove when Gateway handles API key validation
    private readonly IDeviceApiKeyReadRepository _repository;

    public ApiKeyService(IDeviceApiKeyReadRepository repository)
        => _repository = repository;

    public async Task<Guid?> ValidateAsync(string apiKey, CancellationToken ct = default)
    {
        if (string.IsNullOrEmpty(apiKey))
            return null;

        var parts = apiKey.Split('_');
        if (parts.Length < 2)
            return null;

        var prefix = $"{parts[0]}_{parts[1]}";
        var keyHash = HashKey(apiKey);

        var result = await _repository.FindByPrefixAsync(prefix, ct);
        if (result is null)
            return null;

        var (storedPrefix, storedHash, expiresAt, deviceId) = result.Value;

        if (storedHash != keyHash)
            return null;

        if (expiresAt.HasValue && expiresAt.Value < DateTime.UtcNow)
            return null;

        await _repository.UpdateLastUsedAtAsync(prefix, ct);

        return deviceId;
    }

    private static string HashKey(string key)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(key));
        return Convert.ToBase64String(bytes);
    }
}