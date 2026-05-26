using AutoMapper;
using IoT.Application.Commands.DeviceCommands.CreateDeviceCommand;
using IoT.Contracts.DeviceCommands;
using IoT.Domain.Entities;

namespace IoT.Application.Common.Mappings;

public class DeviceCommandMappingProfile : Profile
{
    public DeviceCommandMappingProfile()
    {
        CreateMap<CreateDeviceCommandRequest, CreateDeviceCommandCommand>()
            .ConstructUsing(src => new CreateDeviceCommandCommand(
                Guid.Empty,
                src.CommandTypeSlug,
                src.Parameters,
                src.Priority,
                src.ExpiresAt));

        CreateMap<DeviceCommand, DeviceCommandResponse>()
            .ForMember(dest => dest.CommandTypeSlug,
                opt => opt.MapFrom(src => src.CommandType.Slug));
    }
}

