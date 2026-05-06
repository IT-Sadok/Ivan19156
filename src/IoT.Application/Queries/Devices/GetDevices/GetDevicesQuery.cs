using IoT.Contracts.Devices;
using IoT.Interfaces.Mediator;
using IoT.Shared.Common;

namespace IoT.Application.Queries.Devices.GetDevices;

public record GetDevicesQuery(
    int Page = 1,
    int PageSize = 20,
    int? Type = null,
    int? AdminStatus = null,
    Guid? ManufacturerId = null) : IRequest<Result<PagedResult<DeviceDto>>>;
