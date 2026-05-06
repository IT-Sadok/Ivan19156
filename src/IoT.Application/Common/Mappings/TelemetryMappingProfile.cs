using AutoMapper;
using IoT.Application.Commands.Telemetry.ProcessTelemetry;
using IoT.Contracts.Telemetry;

namespace IoT.Application.Common.Mappings;

public class TelemetryMappingProfile : Profile
{
    public TelemetryMappingProfile()
    {
        CreateMap<ProcessTelemetryRequest, ProcessTelemetryCommand>();
    }
}
