// IoT.Infrastructure/Repositories/CommandTypeRepository.cs
using IoT.Domain.Entities;
using IoT.Infrastructure.Persistence;
using IoT.Interfaces.Repositories;

namespace IoT.Infrastructure.Repositories;

public class CommandTypeRepository 
    : BaseRepository<CommandType>, ICommandTypeRepository
{
    public CommandTypeRepository(AppDbContext context) : base(context) { }
}