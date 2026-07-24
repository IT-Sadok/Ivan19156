using IoT.Domain.Entities;
using IoT.Domain.Enums;

namespace IoT.Interfaces.Repositories;

public interface IRuleRepository : IRepository<Rule>
{
    Task<IEnumerable<Rule>> GetActiveByDeviceAsync(Guid deviceId, DeviceType deviceType, CancellationToken ct = default);
}