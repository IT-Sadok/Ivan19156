using IoT.Domain.Entities;
using IoT.Domain.Enums;

namespace IoT.Interfaces.Repositories;

public interface IDeviceCommandRepository : IRepository<DeviceCommand>
{
    Task<IEnumerable<DeviceCommand>> GetPendingByDeviceIdAsync(Guid deviceId);
    Task<IEnumerable<DeviceCommand>> GetByDeviceIdAsync(Guid deviceId);
}