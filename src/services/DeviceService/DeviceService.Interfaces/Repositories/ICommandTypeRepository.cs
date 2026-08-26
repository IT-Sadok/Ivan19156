using DeviceService.Domain.Entities;

namespace DeviceService.Interfaces.Repositories;

public interface ICommandTypeRepository : IRepository<CommandType>
{
    Task<CommandType?> GetBySlugAsync(string slug, CancellationToken ct = default);
}
