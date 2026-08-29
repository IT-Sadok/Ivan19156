using MaintenanceService.Interfaces.Repositories;

namespace MaintenanceService.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IMaintenanceRecordRepository MaintenanceRecords { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}