using DeviceService.Domain.Entities;
using DeviceService.Domain.Enums;
using DeviceService.Infrastructure.Persistence;
using DeviceService.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DeviceService.Infrastructure.Repositories;

public class DeviceCommandRepository : BaseRepository<DeviceCommand>, IDeviceCommandRepository
{
    public DeviceCommandRepository(DeviceDbContext context) : base(context) { }

    public async Task<IEnumerable<DeviceCommand>> GetPendingByDeviceIdAsync(
        Guid deviceId,
        CancellationToken ct = default)
        => await DbSet
            .Where(dc => dc.DeviceId == deviceId && dc.Status == CommandStatus.Created)
            .OrderBy(dc => dc.CreatedAt)
            .ToListAsync(ct);

    public async Task<IEnumerable<DeviceCommand>> GetByDeviceIdAsync(
        Guid deviceId,
        CancellationToken ct = default)
        => await DbSet
            .Include(dc => dc.CommandType)
            .Where(dc => dc.DeviceId == deviceId)
            .OrderByDescending(dc => dc.CreatedAt)
            .ToListAsync(ct);
}
