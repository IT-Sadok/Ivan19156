// IoT.Application/Devices/Queries/GetDevices/GetDevicesQuery.cs
using IoT.Application.DTOs;
using IoT.Interfaces.Mediator;
using IoT.Shared.Common;

namespace IoT.Application.Devices.Queries.GetDevices;

public record GetDevicesQuery() : IRequest<Result<IEnumerable<DeviceDto>>>;