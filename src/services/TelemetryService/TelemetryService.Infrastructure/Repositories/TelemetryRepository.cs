using Microsoft.EntityFrameworkCore;
using TelemetryService.Domain.Entities;
using TelemetryService.Domain.ValueObjects;
using TelemetryService.Infrastructure.Persistence;
using TelemetryService.Interfaces.Repositories;

namespace TelemetryService.Infrastructure.Repositories;

public class TelemetryRepository : BaseRepository<TelemetryRecord>, ITelemetryRepository
{
    public TelemetryRepository(TelemetryDbContext context) : base(context) { }

    public async Task<IEnumerable<TelemetryRecord>> GetByDeviceIdAsync(
        Guid deviceId,
        CancellationToken ct = default)
        => await DbSet
            .Where(t => t.DeviceId == DeviceId.From(deviceId))
            .OrderByDescending(t => t.ReceivedAt)
            .ToListAsync(ct);

    public async Task<bool> ExistsAsync(
        Guid deviceId,
        Guid messageId,
        CancellationToken ct = default)
        => await DbSet.AnyAsync(t =>
            t.DeviceId == DeviceId.From(deviceId) &&
            t.MessageId == messageId, ct);
}