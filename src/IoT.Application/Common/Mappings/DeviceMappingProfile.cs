using AutoMapper;
using IoT.Application.Commands.Devices.CreateDevice;
using IoT.Application.Commands.Devices.UpdateDevice;
using IoT.Contracts.Devices;
using IoT.Domain.Entities;

namespace IoT.Application.Common.Mappings;

public class DeviceMappingProfile : Profile
{
    public DeviceMappingProfile()
    {
        CreateMap<CreateDeviceRequest, CreateDeviceCommand>()
            .ConstructUsing(src => new CreateDeviceCommand(
                src.Name,
                src.Type,
                src.ManufacturerId));

        CreateMap<UpdateDeviceRequest, UpdateDeviceCommand>()
            .ConstructUsing(src => new UpdateDeviceCommand(
                Guid.Empty,
                src.Name,
                src.AdminStatus,
                src.ManufacturerId));

        CreateMap<Device, DeviceDto>()
            .ForMember(dest => dest.ManufacturerName,
                opt => opt.MapFrom(src => src.Manufacturer != null 
                    ? src.Manufacturer.Name 
                    : null));
    }
}
