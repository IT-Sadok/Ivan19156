using IoT.Domain.Entities;
using IoT.Domain.Enums;
using IoT.Infrastructure.Persistence;
using IoT.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace IoT.Infrastructure.Repositories;

public class RuleRepository : BaseRepository<Rule>, IRuleRepository
{
    public RuleRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Rule>> GetActiveByDeviceAsync(
        Guid deviceId,
        DeviceType deviceType,
        CancellationToken ct = default)
        => await _dbSet
            .Where(r => r.IsActive &&
                        (r.DeviceId == deviceId || r.DeviceType == deviceType))
            .ToListAsync(ct);
}