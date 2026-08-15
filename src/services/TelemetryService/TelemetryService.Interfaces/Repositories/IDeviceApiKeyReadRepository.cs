namespace TelemetryService.Interfaces.Repositories;

public interface IDeviceApiKeyReadRepository
{
    // TODO: Remove when Gateway handles API key validation 
    Task<(string Prefix, string KeyHash, DateTime? ExpiresAt, Guid DeviceId)?> FindByPrefixAsync(
        string prefix,
        CancellationToken ct = default);
    
    Task UpdateLastUsedAtAsync(string prefix, CancellationToken ct = default);
}