using IoT.Contracts.Telemetry;
using IoT.Domain.Entities;
using IoT.Infrastructure.Persistence;
using IoT.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace IoT.Infrastructure.Repositories;

public class TelemetryRepository
    : BaseRepository<TelemetryRecord>, ITelemetryRepository
{
    public TelemetryRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<TelemetryRecord>> GetByDeviceIdAsync(Guid deviceId, CancellationToken ct = default)
        => await _dbSet
            .Where(t => t.DeviceId == deviceId)
            .OrderByDescending(t => t.ReceivedAt)
            .ToListAsync(ct);

    public async Task<bool> ExistsAsync(Guid deviceId, Guid messageId, CancellationToken ct = default)
        => await _dbSet.AnyAsync(t =>
            t.DeviceId == deviceId &&
            t.MessageId == messageId, ct);
    
    public async Task<IEnumerable<TelemetryRecentSummaryResponse>> GetRecentSummaryAsync(
        DateTimeOffset since,
        CancellationToken ct = default)
        => await _dbSet
            .Where(t => t.ReceivedAt > since)
            .GroupBy(t => t.DeviceId)
            .Select(g => new TelemetryRecentSummaryResponse(
                g.Key,
                g.Count(),
                g.Max(t => t.ReceivedAt)))
            .ToListAsync(ct);
}
