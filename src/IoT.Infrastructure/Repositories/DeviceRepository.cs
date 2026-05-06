// IoT.Infrastructure/Repositories/DeviceRepository.cs
using IoT.Domain.Entities;
using IoT.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace IoT.Infrastructure.Repositories;

public class DeviceRepository : BaseRepository<Device>, IDeviceRepository
{
    public DeviceRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Device>> GetOfflineDevicesAsync()
    {
        var threshold = DateTime.UtcNow.AddMinutes(-5);
        return await _dbSet
            .Where(d => d.LastSeen < threshold || d.LastSeen == null)
            .Where(d => d.AdminStatus == Domain.Enums.DeviceAdminStatus.Active)
            .ToListAsync();
    }

    public async Task<IEnumerable<Device>> GetByWarehouseAsync(Guid warehouseId)
    {
        return await _dbSet
            .Where(d => d.Locations.Any(l =>
                l.WarehouseId == warehouseId &&
                l.RemovedAt == null))
            .ToListAsync();
    }

    public async Task<Device?> GetWithDetailsAsync(Guid id)
    {
        return await _dbSet
            .Include(d => d.Manufacturer)
            .Include(d => d.Locations)
            .ThenInclude(l => l.Warehouse)
            .Include(d => d.MaintenanceRecords)
            .FirstOrDefaultAsync(d => d.Id == id);
    }
}