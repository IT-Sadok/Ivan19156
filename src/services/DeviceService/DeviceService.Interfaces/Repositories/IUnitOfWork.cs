using DeviceService.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace DeviceService.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IDeviceRepository Devices { get; }
    IDeviceApiKeyRepository DeviceApiKeys { get; }
    IDeviceCommandRepository DeviceCommands { get; }
    ICommandTypeRepository CommandTypes { get; }
    IAlertRepository Alerts { get; }
    IRuleRepository Rules { get; }
    IManufacturerRepository Manufacturers { get; }
    IWarehouseRepository Warehouses { get; }

    Task<int> SaveChangesAsync(CancellationToken ct = default);
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct = default);
}
