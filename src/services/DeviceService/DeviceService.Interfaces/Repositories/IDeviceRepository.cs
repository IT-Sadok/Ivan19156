using DeviceService.Domain.Entities;
using DeviceService.Domain.Enums;

namespace DeviceService.Interfaces.Repositories;

public interface IDeviceRepository : IRepository<Device>
{
    Task<(IEnumerable<Device> Items, int TotalCount)> GetPagedAsync(
        int page, int pageSize, DeviceType? type = null, CancellationToken ct = default);
}