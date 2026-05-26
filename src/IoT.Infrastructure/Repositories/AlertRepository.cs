using IoT.Domain.Entities;
using IoT.Domain.Enums;
using IoT.Infrastructure.Persistence;
using IoT.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace IoT.Infrastructure.Repositories;

public class AlertRepository : BaseRepository<Alert>, IAlertRepository
{
    public AlertRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Alert>> GetActiveByDeviceAsync(
        Guid deviceId,
        CancellationToken ct = default)
        => await _dbSet
            .Where(a => a.DeviceId == deviceId && a.Status == AlertStatus.New)
            .ToListAsync(ct);
    public async Task<IEnumerable<Alert>> GetActiveAsync(CancellationToken ct = default)
        => await _dbSet
            .Where(a => a.Status == AlertStatus.New)
            .Include(a => a.Device)
            .Include(a => a.Rule)
            .ToListAsync(ct);
}