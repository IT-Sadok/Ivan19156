using DeviceService.Domain.Entities;
using DeviceService.Domain.Enums;

namespace DeviceService.Interfaces.Repositories;

public interface IRuleRepository : IRepository<Rule>
{
    Task<IEnumerable<Rule>> GetActiveByDeviceAsync(Guid deviceId, DeviceType? deviceType, CancellationToken ct = default);
}
