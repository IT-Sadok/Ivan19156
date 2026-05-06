// IoT.Interfaces/IUnitOfWork.cs
using IoT.Interfaces.Repositories;

namespace IoT.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IDeviceRepository Devices { get; }
    IDeviceCommandRepository DeviceCommands { get; }
    ICommandTypeRepository CommandTypes { get; }
    ITelemetryRepository Telemetry { get; }
    
    Task<int> SaveChangesAsync(CancellationToken ct = default);
    Task BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
}