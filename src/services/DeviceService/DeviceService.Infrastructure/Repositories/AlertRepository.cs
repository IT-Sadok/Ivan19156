using DeviceService.Domain.Entities;
using DeviceService.Domain.Enums;
using DeviceService.Infrastructure.Persistence;
using DeviceService.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DeviceService.Infrastructure.Repositories;

public class AlertRepository : BaseRepository<Alert>, IAlertRepository
{
    public AlertRepository(DeviceDbContext context) : base(context) { }

    public async Task<IEnumerable<Alert>> GetActiveByDeviceAsync(
        Guid deviceId,
        CancellationToken ct = default)
        => await DbSet
            .Where(a => a.DeviceId == deviceId && a.Status == AlertStatus.New)
            .ToListAsync(ct);

    public async Task<IEnumerable<Alert>> GetByDeviceAsync(
        Guid deviceId,
        CancellationToken ct = default)
        => await DbSet
            .Include(a => a.Rule)
            .Where(a => a.DeviceId == deviceId)
            .ToListAsync(ct);
}
