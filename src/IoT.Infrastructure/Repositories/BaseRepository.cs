using System.Linq.Expressions;
using IoT.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace IoT.Infrastructure.Repositories;

public class BaseRepository<T> : IRepository<T> where T : class
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public BaseRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _dbSet.FindAsync(new object[] { id }, ct);

    public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken ct = default)
        => await _dbSet.ToListAsync(ct);

    public virtual IQueryable<T> Filter(
        Expression<Func<T, bool>>? predicate = null,
        bool noTracking = false)
    {
        IQueryable<T> query = noTracking
            ? _dbSet.AsNoTracking()
            : _dbSet;

        if (predicate != null)
            query = query.Where(predicate);

        return query;
    }

    public virtual async Task<T?> FirstOrDefaultAsync(
        Expression<Func<T, bool>> predicate,
        bool noTracking = false,
        CancellationToken ct = default)
    {
        IQueryable<T> query = noTracking
            ? _dbSet.AsNoTracking()
            : _dbSet;

        return await query.FirstOrDefaultAsync(predicate, ct);
    }

    public virtual async Task<T?> LastOrDefaultAsync(
        Expression<Func<T, bool>> predicate,
        bool noTracking = false,
        CancellationToken ct = default)
    {
        IQueryable<T> query = noTracking
            ? _dbSet.AsNoTracking()
            : _dbSet;

        return await query.Where(predicate).LastOrDefaultAsync(ct);
    }

    public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
        => await _dbSet.AnyAsync(predicate, ct);

    public virtual async Task AddAsync(T entity, CancellationToken ct = default)
        => await _dbSet.AddAsync(entity, ct);

    public virtual async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken ct = default)
        => await _dbSet.AddRangeAsync(entities, ct);

    public virtual Task UpdateAsync(T entity, CancellationToken ct = default)
    {
        _dbSet.Update(entity);
        return Task.CompletedTask;
    }

    public virtual Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken ct = default)
    {
        _dbSet.UpdateRange(entities);
        return Task.CompletedTask;
    }

    public virtual Task DeleteAsync(T entity, CancellationToken ct = default)
    {
        _dbSet.Remove(entity);
        return Task.CompletedTask;
    }

    public virtual Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken ct = default)
    {
        _dbSet.RemoveRange(entities);
        return Task.CompletedTask;
    }
}
