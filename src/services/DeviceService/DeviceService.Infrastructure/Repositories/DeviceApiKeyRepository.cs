using DeviceService.Domain.Entities;
using DeviceService.Infrastructure.Persistence;
using DeviceService.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DeviceService.Infrastructure.Repositories;

public class DeviceApiKeyRepository : BaseRepository<DeviceApiKey>, IDeviceApiKeyRepository
{
    public DeviceApiKeyRepository(DeviceDbContext context) : base(context) { }

    public async Task<DeviceApiKey?> FindByPrefixAsync(string prefix, CancellationToken ct = default)
        => await FirstOrDefaultAsync(k => k.Prefix == prefix, noTracking: false, ct);
    
    public async Task<IEnumerable<DeviceApiKey>> GetByDeviceIdAsync(Guid deviceId, CancellationToken ct = default)
        => await DbSet
            .Where(k => k.DeviceId == deviceId)
            .OrderByDescending(k => k.CreatedAt)
            .ToListAsync(ct);
}
