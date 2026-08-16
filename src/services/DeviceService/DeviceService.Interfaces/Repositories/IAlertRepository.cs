using DeviceService.Domain.Entities;

namespace DeviceService.Interfaces.Repositories;

public interface IAlertRepository : IRepository<Alert>
{
    Task<IEnumerable<Alert>> GetActiveByDeviceAsync(Guid deviceId, CancellationToken ct = default);
    Task<IEnumerable<Alert>> GetByDeviceAsync(Guid deviceId, CancellationToken ct = default);
}
