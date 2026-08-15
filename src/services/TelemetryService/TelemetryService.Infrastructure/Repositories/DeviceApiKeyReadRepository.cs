using Microsoft.EntityFrameworkCore;
using TelemetryService.Infrastructure.Persistence;
using TelemetryService.Interfaces.Repositories;

namespace TelemetryService.Infrastructure.Repositories;

public class DeviceApiKeyReadRepository : IDeviceApiKeyReadRepository
{
    // TODO: Remove when Gateway handles API key validation
    private readonly TelemetryDbContext _context;

    public DeviceApiKeyReadRepository(TelemetryDbContext context)
        => _context = context;

    public async Task<(string Prefix, string KeyHash, DateTime? ExpiresAt, Guid DeviceId)?> FindByPrefixAsync(
        string prefix,
        CancellationToken ct = default)
        => await _context.DeviceApiKeys
            .Where(k => k.Prefix == prefix)
            .Select(k => new ValueTuple<string, string, DateTime?, Guid>(
                k.Prefix, k.KeyHash, k.ExpiresAt, k.DeviceId))
            .FirstOrDefaultAsync(ct);

    public async Task UpdateLastUsedAtAsync(string prefix, CancellationToken ct = default)
        => await _context.DeviceApiKeys
            .Where(k => k.Prefix == prefix)
            .ExecuteUpdateAsync(s =>
                s.SetProperty(k => k.LastUsedAt, DateTime.UtcNow), ct);
}