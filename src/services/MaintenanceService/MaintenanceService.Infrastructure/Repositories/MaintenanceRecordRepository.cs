using MaintenanceService.Domain.Entities;
using MaintenanceService.Domain.ValueObjects;
using MaintenanceService.Infrastructure.Persistence;
using MaintenanceService.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Pgvector;
using Pgvector.EntityFrameworkCore;

namespace MaintenanceService.Infrastructure.Repositories;

public class MaintenanceRecordRepository : BaseRepository<MaintenanceRecord>, IMaintenanceRecordRepository
{
    public MaintenanceRecordRepository(MaintenanceDbContext context) : base(context) { }

    public async Task<IEnumerable<MaintenanceRecord>> GetByDeviceIdAsync(
        Guid deviceId,
        CancellationToken ct = default)
        => await DbSet
            .Where(r => r.DeviceId == DeviceId.From(deviceId))
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
            .ToListAsync(ct);
    }
}