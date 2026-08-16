using DeviceService.Domain.Entities;
using DeviceService.Infrastructure.Persistence;
using DeviceService.Interfaces.Repositories;

namespace DeviceService.Infrastructure.Repositories;

public class ManufacturerRepository : BaseRepository<Manufacturer>, IManufacturerRepository
{
    public ManufacturerRepository(DeviceDbContext context) : base(context) { }
}
