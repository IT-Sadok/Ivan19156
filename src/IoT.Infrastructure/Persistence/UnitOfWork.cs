using IoT.Infrastructure.Repositories;
using IoT.Interfaces;
using IoT.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace IoT.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private bool _disposed;

    public UnitOfWork(
        AppDbContext context,
        IDeviceRepository devices,
        IDeviceCommandRepository deviceCommands,
        ICommandTypeRepository commandTypes,
        ITelemetryRepository telemetry,
        IDeviceApiKeyRepository deviceApiKeys,
        IRuleRepository rules,
        IAlertRepository alerts)
    {
        _context = context;
        Devices = devices;
        DeviceCommands = deviceCommands;
        CommandTypes = commandTypes;
        Telemetry = telemetry;
        DeviceApiKeys = deviceApiKeys;
        Rules = rules;
        Alerts = alerts;
    }

    public IDeviceRepository Devices { get; }
    public IDeviceCommandRepository DeviceCommands { get; }
    public ICommandTypeRepository CommandTypes { get; }
    public ITelemetryRepository Telemetry { get; }
    public IDeviceApiKeyRepository DeviceApiKeys { get; }
    public IRuleRepository Rules { get; }
    public IAlertRepository Alerts { get; }

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

    ~UnitOfWork() => Dispose(false);
}
