using DeviceService.Domain.Entities;
using DeviceService.Domain.Enums;
using DeviceService.Infrastructure.Persistence;
using DeviceService.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DeviceService.Infrastructure.Repositories;

public class DeviceRepository : BaseRepository<Device>, IDeviceRepository
{
    public DeviceRepository(DeviceDbContext context) : base(context) { }

    public async Task<(IEnumerable<Device> Items, int TotalCount)> GetPagedAsync(
        int page, int pageSize, DeviceType? type = null, CancellationToken ct = default)
    {
        var query = DbSet.AsQueryable();

        if (type.HasValue)
            query = query.Where(d => d.Type == type.Value);

        var totalCount = await query.CountAsync(ct);
        var items = await query
            .OrderBy(d => d.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, totalCount);
    }
}