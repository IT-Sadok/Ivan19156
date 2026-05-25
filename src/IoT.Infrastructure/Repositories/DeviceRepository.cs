using IoT.Domain.Entities;
using IoT.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace IoT.Infrastructure.Repositories;

public class DeviceRepository : BaseRepository<Device>, IDeviceRepository
{
    private readonly TimeProvider _timeProvider;

    public DeviceRepository(AppDbContext context, TimeProvider timeProvider) : base(context)
        => _timeProvider = timeProvider;

    public async Task<IEnumerable<Device>> GetOfflineDevicesAsync(CancellationToken ct = default)
    {
        var threshold = _timeProvider.GetUtcNow().AddMinutes(-5).UtcDateTime;
        return await _dbSet
            .Where(d => d.LastSeen < threshold || d.LastSeen == null)
            .Where(d => d.AdminStatus == Domain.Enums.DeviceAdminStatus.Active)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<Device>> GetByWarehouseAsync(Guid warehouseId, CancellationToken ct = default)
    {
        return await _dbSet
            .Where(d => d.Locations.Any(l =>
                l.WarehouseId == warehouseId &&
                l.RemovedAt == null))
            .ToListAsync(ct);
    }

    public async Task<Device?> GetWithDetailsAsync(Guid id, CancellationToken ct = default)
    {
        return await _dbSet
            .Include(d => d.Manufacturer)
            .Include(d => d.Locations)
            .ThenInclude(l => l.Warehouse)
            .Include(d => d.MaintenanceRecords)
            .FirstOrDefaultAsync(d => d.Id == id, ct);
    }
    public async Task<IEnumerable<Device>> GetAllWithStatusAsync(CancellationToken ct = default)
        => await _dbSet
            .Include(d => d.Manufacturer)
            .ToListAsync(ct);
}
