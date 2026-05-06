using IoT.Domain.Entities;
using IoT.Domain.Enums;
using IoT.Infrastructure.Persistence;
using IoT.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace IoT.Infrastructure.Repositories;

public class DeviceCommandRepository 
    : BaseRepository<DeviceCommand>, IDeviceCommandRepository
{
    public DeviceCommandRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<DeviceCommand>> GetPendingByDeviceIdAsync(Guid deviceId)
        => await _dbSet
            .Where(c => c.DeviceId == deviceId && c.Status == CommandStatus.Created)
            .OrderByDescending(c => c.Priority)
            .ThenBy(c => c.CreatedAt)
            .ToListAsync();

    public async Task<IEnumerable<DeviceCommand>> GetByDeviceIdAsync(Guid deviceId)
        => await _dbSet
            .Include(c => c.CommandType)
            .Include(c => c.IssuedBy)
            .Where(c => c.DeviceId == deviceId)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
}