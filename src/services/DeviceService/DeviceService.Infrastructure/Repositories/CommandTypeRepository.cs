using DeviceService.Domain.Entities;
using DeviceService.Infrastructure.Persistence;
using DeviceService.Interfaces.Repositories;

namespace DeviceService.Infrastructure.Repositories;

public class CommandTypeRepository : BaseRepository<CommandType>, ICommandTypeRepository
{
    public CommandTypeRepository(DeviceDbContext context) : base(context) { }

    public async Task<CommandType?> GetBySlugAsync(string slug, CancellationToken ct = default)
        => await FirstOrDefaultAsync(commandType => commandType.Slug == slug, noTracking: false, ct);
}
