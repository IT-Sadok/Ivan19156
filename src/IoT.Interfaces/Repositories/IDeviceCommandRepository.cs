using IoT.Domain.Entities;

namespace IoT.Interfaces.Repositories;

public interface IDeviceCommandRepository : IRepository<DeviceCommand>
{
    Task<IEnumerable<DeviceCommand>> GetPendingByDeviceIdAsync(Guid deviceId, CancellationToken ct = default);
    Task<IEnumerable<DeviceCommand>> GetByDeviceIdAsync(Guid deviceId, CancellationToken ct = default);
}
