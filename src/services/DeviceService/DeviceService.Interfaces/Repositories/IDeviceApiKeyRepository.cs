using DeviceService.Domain.Entities;

namespace DeviceService.Interfaces.Repositories;

public interface IDeviceApiKeyRepository : IRepository<DeviceApiKey>
{
    Task<DeviceApiKey?> FindByPrefixAsync(string prefix, CancellationToken ct = default);
    Task<IEnumerable<DeviceApiKey>> GetByDeviceIdAsync(Guid deviceId, CancellationToken ct = default);
}
