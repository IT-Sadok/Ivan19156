using System.Security.Cryptography;
using System.Text;
using DeviceService.Domain.Entities;
using DeviceService.Interfaces;
using DeviceService.Interfaces.Services;
using IoT.Contracts.Events;
using MassTransit;

namespace DeviceService.Infrastructure.Services;

public class ApiKeyService : IApiKeyService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublishEndpoint _publishEndpoint;

    public ApiKeyService(IUnitOfWork unitOfWork, IPublishEndpoint publishEndpoint)
    {
        _unitOfWork = unitOfWork;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<string> GenerateAsync(Guid deviceId, CancellationToken ct = default)
    {
        var rawBytes = RandomNumberGenerator.GetBytes(32);
        var rawKey = Convert.ToHexString(rawBytes).ToLowerInvariant(); 
        var prefix = $"iot_{rawKey[..8]}";
        var fullKey = $"{prefix}_{rawKey}";
        var hash = HashKey(fullKey);

        var apiKey = new DeviceApiKey
        {
            DeviceId = deviceId,
            Prefix = prefix,
            KeyHash = hash,
            IsActive = true,
            ExpiresAt = DateTime.UtcNow.AddYears(1)
        };

        await _unitOfWork.DeviceApiKeys.AddAsync(apiKey, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        await _publishEndpoint.Publish(
            new ApiKeyGeneratedEvent(deviceId, apiKey.Id, DateTime.UtcNow),
            ct);

        return fullKey;
    }

    public async Task<Guid?> ValidateAsync(string apiKey, CancellationToken ct = default)
    {
        var parts = apiKey.Split('_');
        if (parts.Length < 2)
            return null;

        var prefix = $"{parts[0]}_{parts[1]}";
        var hash = HashKey(apiKey);

        var storedKey = await _unitOfWork.DeviceApiKeys.FindByPrefixAsync(prefix, ct);

        if (storedKey is null)
            return null;

        if (storedKey.KeyHash != hash)
            return null;

        if (!storedKey.IsActive)
            return null;

        if (storedKey.ExpiresAt.HasValue && storedKey.ExpiresAt.Value < DateTime.UtcNow)
            return null;

        return storedKey.DeviceId;
    }

    public async Task<bool> RevokeAsync(Guid apiKeyId, CancellationToken ct = default)
    {
        var apiKey = await _unitOfWork.DeviceApiKeys.GetByIdAsync(apiKeyId, ct);
        if (apiKey is null)
            return false;

        apiKey.IsActive = false;
        await _unitOfWork.DeviceApiKeys.UpdateAsync(apiKey, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return true;
    }

    private static string HashKey(string key)
        => Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(key)));
}
