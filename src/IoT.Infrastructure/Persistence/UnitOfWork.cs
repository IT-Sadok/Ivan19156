using IoT.Infrastructure.Repositories;
using IoT.Interfaces;
using IoT.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace IoT.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private readonly TimeProvider _timeProvider;
    private IDeviceRepository? _devices;
    private IDeviceCommandRepository? _deviceCommands;
    private ICommandTypeRepository? _commandTypes;
    private ITelemetryRepository? _telemetry;
    private bool _disposed;

    public UnitOfWork(AppDbContext context, TimeProvider timeProvider)
    {
        _context = context;
        _timeProvider = timeProvider;
    }

    public IDeviceRepository Devices
        => _devices ??= new DeviceRepository(_context, _timeProvider);

    public IDeviceCommandRepository DeviceCommands
        => _deviceCommands ??= new DeviceCommandRepository(_context);

    public ICommandTypeRepository CommandTypes
        => _commandTypes ??= new CommandTypeRepository(_context);

    public ITelemetryRepository Telemetry
        => _telemetry ??= new TelemetryRepository(_context);

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
        if (_disposed)
            return;

        if (disposing)
            _context.Dispose();

        _disposed = true;
    }

    ~UnitOfWork()
    {
        Dispose(false);
    }
}
