using TelemetryService.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace TelemetryService.Interfaces;

public interface IUnitOfWork : IDisposable
{
    ITelemetryRepository Telemetry { get; }

    Task<int> SaveChangesAsync(CancellationToken ct = default);
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct = default);
}