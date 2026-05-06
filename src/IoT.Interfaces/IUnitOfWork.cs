// IoT.Interfaces/IUnitOfWork.cs
using IoT.Interfaces.Repositories;

namespace IoT.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IDeviceRepository Devices { get; }
    
    Task<int> SaveChangesAsync(CancellationToken ct = default);
    Task BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
}