using IoT.Domain.Entities;

namespace IoT.Interfaces.Repositories;

public interface IAlertRepository : IRepository<Alert>
{
    Task<IEnumerable<Alert>> GetActiveByDeviceAsync(Guid deviceId, CancellationToken ct = default);
}