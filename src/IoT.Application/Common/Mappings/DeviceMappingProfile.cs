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
        CreateMap<Device, DeviceDto>()
            .ForMember(d => d.ManufacturerName,
                o => o.MapFrom(s => s.Manufacturer == null ? null : s.Manufacturer.Name));

        CreateMap<CreateDeviceDto, CreateDeviceCommand>();
        CreateMap<CreateDeviceRequest, CreateDeviceCommand>();

        CreateMap<UpdateDeviceDto, UpdateDeviceCommand>()
            .ForCtorParam("id", o => o.MapFrom(_ => Guid.Empty));
        CreateMap<UpdateDeviceRequest, UpdateDeviceCommand>()
            .ForCtorParam("id", o => o.MapFrom(_ => Guid.Empty));
    }
}
