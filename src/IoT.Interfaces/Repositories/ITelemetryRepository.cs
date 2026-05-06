using IoT.Domain.Entities;

namespace IoT.Interfaces.Repositories;

public interface ITelemetryRepository : IRepository<TelemetryRecord>
{
    Task<IEnumerable<TelemetryRecord>> GetByDeviceIdAsync(Guid deviceId);
    Task<bool> ExistsAsync(Guid deviceId, Guid messageId);
}