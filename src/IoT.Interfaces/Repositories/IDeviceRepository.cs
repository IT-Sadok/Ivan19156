// IoT.Interfaces/Repositories/IDeviceRepository.cs
using IoT.Domain.Entities;

namespace IoT.Interfaces.Repositories;

public interface IDeviceRepository : IRepository<Device>
{
    Task<IEnumerable<Device>> GetOfflineDevicesAsync();
    Task<IEnumerable<Device>> GetByWarehouseAsync(Guid warehouseId);
    Task<Device?> GetWithDetailsAsync(Guid id);
}