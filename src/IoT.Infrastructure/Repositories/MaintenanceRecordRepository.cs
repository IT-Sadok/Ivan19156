using IoT.Domain.Entities;
using IoT.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Pgvector;
using Pgvector.EntityFrameworkCore;

namespace IoT.Infrastructure.Repositories;

public class MaintenanceRecordRepository(AppDbContext context) 
    : BaseRepository<MaintenanceRecord>(context), IMaintenanceRecordRepository
{
    public async Task<IEnumerable<MaintenanceRecord>> GetByDeviceIdAsync(
        Guid deviceId,
        CancellationToken ct = default)
        => await Filter(r => r.DeviceId == deviceId)
            .Include(r => r.Device)
            .OrderByDescending(r => r.PerformedAt)
            .ToListAsync(ct);
    
    public async Task<IEnumerable<MaintenanceRecord>> SearchByEmbeddingAsync(
        float[] queryEmbedding,
        int limit = 5,
        CancellationToken ct = default)
    {
        var vector = new Vector(queryEmbedding);
        return await _context.MaintenanceRecords
            .Where(r => r.NotesEmbedding != null)
            .OrderBy(r => r.NotesEmbedding!.CosineDistance(vector))
            .Take(limit)
            .Include(r => r.Device)
            .ToListAsync(ct);
    }
}