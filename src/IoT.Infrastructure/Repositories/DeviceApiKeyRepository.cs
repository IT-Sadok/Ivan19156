using IoT.Domain.Entities;
using IoT.Infrastructure.Persistence;
using IoT.Interfaces.Repositories;

namespace IoT.Infrastructure.Repositories;

public class DeviceApiKeyRepository
    : BaseRepository<DeviceApiKey>, IDeviceApiKeyRepository
{
    public DeviceApiKeyRepository(AppDbContext context) : base(context) { }
}