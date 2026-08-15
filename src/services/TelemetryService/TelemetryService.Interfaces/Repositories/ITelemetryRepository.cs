using TelemetryService.Domain.Entities;

namespace TelemetryService.Interfaces.Repositories;

public interface ITelemetryRepository : IRepository<TelemetryRecord>
{
    Task<IEnumerable<TelemetryRecord>> GetByDeviceIdAsync(Guid deviceId, CancellationToken ct = default);
    Task<bool> ExistsAsync(Guid deviceId, Guid messageId, CancellationToken ct = default);
}