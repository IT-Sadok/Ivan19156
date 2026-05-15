using IoT.Domain.Entities;

namespace IoT.Interfaces.Repositories;

public interface IDeviceRepository : IRepository<Device>
{
    Task<IEnumerable<Device>> GetOfflineDevicesAsync(CancellationToken ct = default);
    Task<IEnumerable<Device>> GetByWarehouseAsync(Guid warehouseId, CancellationToken ct = default);
    Task<Device?> GetWithDetailsAsync(Guid id, CancellationToken ct = default);
}
