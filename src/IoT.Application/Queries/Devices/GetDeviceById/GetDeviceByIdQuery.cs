using IoT.Contracts.Devices;
using IoT.Shared.Mediator;
using IoT.Shared.Common;

namespace IoT.Application.Queries.Devices.GetDeviceById;

public record GetDeviceByIdQuery(Guid Id) : IRequest<Result<DeviceResponse>>;
