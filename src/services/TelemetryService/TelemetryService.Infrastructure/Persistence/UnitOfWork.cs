using Microsoft.EntityFrameworkCore.Storage;
using TelemetryService.Interfaces;
using TelemetryService.Interfaces.Repositories;

namespace TelemetryService.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly TelemetryDbContext _context;
    private bool _disposed;

    public UnitOfWork(TelemetryDbContext context, ITelemetryRepository telemetry)
    {
        _context = context;
        Telemetry = telemetry;
    }

    public ITelemetryRepository Telemetry { get; }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        => await _context.SaveChangesAsync(ct);

    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct = default)
        => _context.Database.BeginTransactionAsync(ct);

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