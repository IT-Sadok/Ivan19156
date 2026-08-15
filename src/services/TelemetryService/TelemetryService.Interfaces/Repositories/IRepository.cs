using System.Linq.Expressions;

namespace TelemetryService.Interfaces.Repositories;

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken ct = default);
    IQueryable<T> Filter(
        Expression<Func<T, bool>>? predicate = null,
        bool noTracking = false);
    Task<T?> FirstOrDefaultAsync(
        Expression<Func<T, bool>> predicate,
        bool noTracking = false,
        CancellationToken ct = default);
    Task<T?> LastOrDefaultAsync(
        Expression<Func<T, bool>> predicate,
        bool noTracking = false,
        CancellationToken ct = default);
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);
    Task AddAsync(T entity, CancellationToken ct = default);
    Task AddRangeAsync(IEnumerable<T> entities, CancellationToken ct = default);
    Task UpdateAsync(T entity, CancellationToken ct = default);
    Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken ct = default);
    Task DeleteAsync(T entity, CancellationToken ct = default);
    Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken ct = default);
}