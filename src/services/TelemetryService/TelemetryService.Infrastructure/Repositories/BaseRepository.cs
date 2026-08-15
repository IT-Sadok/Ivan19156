using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TelemetryService.Infrastructure.Persistence;
using TelemetryService.Interfaces.Repositories;

public abstract class BaseRepository<T> : IRepository<T> where T : class
{
    protected readonly TelemetryDbContext _context;

    protected BaseRepository(TelemetryDbContext context)
        => _context = context;

    protected DbSet<T> DbSet => _context.Set<T>();

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await DbSet.FindAsync([id], ct);

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken ct = default)
        => await DbSet.ToListAsync(ct);

    public IQueryable<T> Filter(
        Expression<Func<T, bool>>? predicate = null,
        bool noTracking = false)
    {
        var query = noTracking ? DbSet.AsNoTracking() : DbSet.AsQueryable();
        return predicate == null ? query : query.Where(predicate);
    }

    public async Task<T?> FirstOrDefaultAsync(
        Expression<Func<T, bool>> predicate,
        bool noTracking = false,
        CancellationToken ct = default)
    {
        var query = noTracking ? DbSet.AsNoTracking() : DbSet.AsQueryable();
        return await query.FirstOrDefaultAsync(predicate, ct);
    }

    public async Task<T?> LastOrDefaultAsync(
        Expression<Func<T, bool>> predicate,
        bool noTracking = false,
        CancellationToken ct = default)
    {
        var query = noTracking ? DbSet.AsNoTracking() : DbSet.AsQueryable();
        return await query.LastOrDefaultAsync(predicate, ct);
    }

    public async Task<bool> AnyAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken ct = default)
        => await DbSet.AnyAsync(predicate, ct);

    public async Task AddAsync(T entity, CancellationToken ct = default)
        => await DbSet.AddAsync(entity, ct);

    public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken ct = default)
        => await DbSet.AddRangeAsync(entities, ct);

    public Task UpdateAsync(T entity, CancellationToken ct = default)
    {
        DbSet.Update(entity);
        return Task.CompletedTask;
    }

    public Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken ct = default)
    {
        DbSet.UpdateRange(entities);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(T entity, CancellationToken ct = default)
    {
        DbSet.Remove(entity);
        return Task.CompletedTask;
    }

    public Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken ct = default)
    {
        DbSet.RemoveRange(entities);
        return Task.CompletedTask;
    }
}