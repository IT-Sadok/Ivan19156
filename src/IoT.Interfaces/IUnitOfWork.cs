using IoT.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace IoT.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IDeviceRepository Devices { get; }
    IDeviceCommandRepository DeviceCommands { get; }
    ICommandTypeRepository CommandTypes { get; }
    ITelemetryRepository Telemetry { get; }

    Task<int> SaveChangesAsync(CancellationToken ct = default);
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct = default);
}
