using IoT.Domain.Entities;

namespace IoT.Interfaces.Repositories;

public interface IMaintenanceRecordRepository : IRepository<MaintenanceRecord>
{
    Task<IEnumerable<MaintenanceRecord>> GetByDeviceIdAsync(Guid deviceId, CancellationToken ct = default);
    Task<IEnumerable<MaintenanceRecord>> SearchByEmbeddingAsync(float[] queryEmbedding, int limit = 5, CancellationToken ct = default);
}