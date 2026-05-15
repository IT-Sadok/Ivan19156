using IoT.Contracts.Devices;
using IoT.Interfaces.Mediator;
using IoT.Shared.Common;

namespace IoT.Application.Queries.Devices.GetDeviceById;

public record GetDeviceByIdQuery(Guid Id) : IRequest<Result<DeviceResponse>>;
