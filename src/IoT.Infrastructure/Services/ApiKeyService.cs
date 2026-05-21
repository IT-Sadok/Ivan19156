using System.Security.Cryptography;
using System.Text;
using IoT.Domain.Entities;
using IoT.Interfaces;
using IoT.Interfaces.Services;


namespace IoT.Infrastructure.Services;

public class ApiKeyService : IApiKeyService
{
    private readonly IUnitOfWork _unitOfWork;

    public ApiKeyService(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<string> GenerateAsync(Guid deviceId, CancellationToken ct = default)
    {
        var rawKey = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        var prefix = $"iot_{rawKey[..8]}";
        var fullKey = $"{prefix}_{rawKey}";
        
        var keyHash = HashKey(fullKey);

        var apiKey = new DeviceApiKey
        {
            Id = Guid.NewGuid(),
            DeviceId = deviceId,
            Prefix = prefix,
            KeyHash = keyHash,
            ExpiresAt = DateTime.UtcNow.AddYears(1)
        };

        await _unitOfWork.DeviceApiKeys.AddAsync(apiKey, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return fullKey;
    }

    public async Task<Guid?> ValidateAsync(string apiKey, CancellationToken ct = default)
    {
        if (string.IsNullOrEmpty(apiKey))
            return null;

        var parts = apiKey.Split('_');
        if (parts.Length < 2)
            return null;

        var prefix = $"{parts[0]}_{parts[1]}";
        var keyHash = HashKey(apiKey);

        var deviceApiKey = await _unitOfWork.DeviceApiKeys
            .FirstOrDefaultAsync(k =>
                k.Prefix == prefix &&
                k.KeyHash == keyHash &&
                (k.ExpiresAt == null || k.ExpiresAt > DateTime.UtcNow),
                noTracking: false,
                ct);

        if (deviceApiKey == null)
            return null;

        deviceApiKey.LastUsedAt = DateTime.UtcNow;
        await _unitOfWork.DeviceApiKeys.UpdateAsync(deviceApiKey, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return deviceApiKey.DeviceId;
    }

    private static string HashKey(string key)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(key));
        return Convert.ToBase64String(bytes);
    }
}