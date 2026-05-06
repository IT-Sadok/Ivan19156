// IoT.Infrastructure/Repositories/BaseRepository.cs
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

    public async Task<T?> GetByIdAsync(Guid id)
        => await _dbSet.FindAsync(id);

    public async Task<IEnumerable<T>> GetAllAsync()
        => await _dbSet.ToListAsync();

    public IQueryable<T> Filter(
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

    public async Task<T?> FirstOrDefaultAsync(
        Expression<Func<T, bool>> predicate,
        bool noTracking = false)
    {
        IQueryable<T> query = noTracking
            ? _dbSet.AsNoTracking()
            : _dbSet;

        return await query.FirstOrDefaultAsync(predicate);
    }

    public async Task<T?> LastOrDefaultAsync(
        Expression<Func<T, bool>> predicate,
        bool noTracking = false)
    {
        IQueryable<T> query = noTracking
            ? _dbSet.AsNoTracking()
            : _dbSet;

        return await query.Where(predicate).LastOrDefaultAsync();
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        => await _dbSet.AnyAsync(predicate);

    public async Task AddAsync(T entity)
        => await _dbSet.AddAsync(entity);

    public async Task AddRangeAsync(IEnumerable<T> entities)
        => await _dbSet.AddRangeAsync(entities);

    public Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        return Task.CompletedTask;
    }

    public Task UpdateRangeAsync(IEnumerable<T> entities)
    {
        _dbSet.UpdateRange(entities);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
        return Task.CompletedTask;
    }

    public Task DeleteRangeAsync(IEnumerable<T> entities)
    {
        _dbSet.RemoveRange(entities);
        return Task.CompletedTask;
    }
}