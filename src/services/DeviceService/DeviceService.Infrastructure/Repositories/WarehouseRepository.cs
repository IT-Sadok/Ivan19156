using DeviceService.Domain.Entities;
using DeviceService.Infrastructure.Persistence;
using DeviceService.Interfaces.Repositories;

namespace DeviceService.Infrastructure.Repositories;

public class WarehouseRepository : BaseRepository<Warehouse>, IWarehouseRepository
{
    public WarehouseRepository(DeviceDbContext context) : base(context) { }
}
