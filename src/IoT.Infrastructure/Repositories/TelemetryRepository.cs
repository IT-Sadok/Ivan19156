using IoT.Domain.Entities;
using IoT.Infrastructure.Persistence;
using IoT.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace IoT.Infrastructure.Repositories;

public class TelemetryRepository 
    : BaseRepository<TelemetryRecord>, ITelemetryRepository
{
    public TelemetryRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<TelemetryRecord>> GetByDeviceIdAsync(Guid deviceId)
        => await _dbSet
            .Where(t => t.DeviceId == deviceId)
            .OrderByDescending(t => t.ReceivedAt)
            .ToListAsync();

    public async Task<bool> ExistsAsync(Guid deviceId, Guid messageId)
        => await _dbSet.AnyAsync(t => 
            t.DeviceId == deviceId && 
            t.MessageId == messageId);
}