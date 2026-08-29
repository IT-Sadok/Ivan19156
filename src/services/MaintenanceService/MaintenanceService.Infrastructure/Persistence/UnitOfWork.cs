using MaintenanceService.Interfaces;
using MaintenanceService.Interfaces.Repositories;

namespace MaintenanceService.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly MaintenanceDbContext _context;
    private bool _disposed;

    public UnitOfWork(MaintenanceDbContext context, IMaintenanceRecordRepository maintenanceRecords)
    {
        _context = context;
        MaintenanceRecords = maintenanceRecords;
    }

    public IMaintenanceRecordRepository MaintenanceRecords { get; }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        => await _context.SaveChangesAsync(ct);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;
        if (disposing) _context.Dispose();
        _disposed = true;
    }

    ~UnitOfWork() => Dispose(false);
}