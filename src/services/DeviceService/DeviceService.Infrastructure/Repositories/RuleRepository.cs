using DeviceService.Domain.Entities;
using DeviceService.Domain.Enums;
using DeviceService.Infrastructure.Persistence;
using DeviceService.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DeviceService.Infrastructure.Repositories;

public class RuleRepository : BaseRepository<Rule>, IRuleRepository
{
    public RuleRepository(DeviceDbContext context) : base(context) { }

    public async Task<IEnumerable<Rule>> GetActiveByDeviceAsync(
        Guid deviceId,
        DeviceType? deviceType,
        CancellationToken ct = default)
        => await DbSet
            .Where(r => r.IsActive &&
                        (r.DeviceId == deviceId ||
                         (deviceType != null && r.DeviceType == deviceType)))
            .ToListAsync(ct);
}
