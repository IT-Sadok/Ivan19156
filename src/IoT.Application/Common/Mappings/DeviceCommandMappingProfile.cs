using AutoMapper;
using IoT.Application.Commands.DeviceCommands.CreateDeviceCommand;
using IoT.Contracts.DeviceCommands;
using IoT.Domain.Entities;

namespace IoT.Application.Common.Mappings;

public class DeviceCommandMappingProfile : Profile
{
    public DeviceCommandMappingProfile()
    {
        CreateMap<DeviceCommand, DeviceCommandDto>()
            .ForMember(d => d.CommandTypeSlug,
                o => o.MapFrom(s => s.CommandType.Slug));

        CreateMap<CreateDeviceCommandRequest, CreateDeviceCommandCommand>()
            .ForCtorParam("deviceId", o => o.MapFrom(_ => Guid.Empty));
    }
}
