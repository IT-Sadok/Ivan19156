// IoT.Application/Devices/Queries/GetDeviceById/GetDeviceByIdQuery.cs
using IoT.Application.DTOs;
using IoT.Interfaces.Mediator;
using IoT.Shared.Common;

namespace IoT.Application.Devices.Queries.GetDeviceById;

public record GetDeviceByIdQuery(Guid Id) : IRequest<Result<DeviceDto>>;