using DeviceService.Domain.Entities;

namespace DeviceService.Interfaces.Repositories;

public interface IDeviceCommandRepository : IRepository<DeviceCommand>
{
    Task<IEnumerable<DeviceCommand>> GetPendingByDeviceIdAsync(Guid deviceId, CancellationToken ct = default);
    Task<IEnumerable<DeviceCommand>> GetByDeviceIdAsync(Guid deviceId, CancellationToken ct = default);
}
