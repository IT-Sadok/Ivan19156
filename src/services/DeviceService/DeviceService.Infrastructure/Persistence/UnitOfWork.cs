using DeviceService.Interfaces;
using DeviceService.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace DeviceService.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly DeviceDbContext _context;
    private bool _disposed;

    public UnitOfWork(
        DeviceDbContext context,
        IDeviceRepository devices,
        IDeviceApiKeyRepository deviceApiKeys,
        IDeviceCommandRepository deviceCommands,
        ICommandTypeRepository commandTypes,
        IAlertRepository alerts,
        IRuleRepository rules,
        IManufacturerRepository manufacturers,
        IWarehouseRepository warehouses)
    {
        _context = context;
        Devices = devices;
        DeviceApiKeys = deviceApiKeys;
        DeviceCommands = deviceCommands;
        CommandTypes = commandTypes;
        Alerts = alerts;
        Rules = rules;
        Manufacturers = manufacturers;
        Warehouses = warehouses;
    }

    public IDeviceRepository Devices { get; }
    public IDeviceApiKeyRepository DeviceApiKeys { get; }
    public IDeviceCommandRepository DeviceCommands { get; }
    public ICommandTypeRepository CommandTypes { get; }
    public IAlertRepository Alerts { get; }
    public IRuleRepository Rules { get; }
    public IManufacturerRepository Manufacturers { get; }
    public IWarehouseRepository Warehouses { get; }

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
