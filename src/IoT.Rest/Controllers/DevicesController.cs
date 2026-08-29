using IoT.Application.Common.Mappings;
using IoT.Application.Common.Mappings;
using IoT.Application.Queries.Devices.GetDeviceById;
using IoT.Application.Queries.Devices.GetDevices;
using IoT.Contracts.Devices;
using IoT.Domain.Constants;
using IoT.Shared.Common;
using IoT.Shared.Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IoT.Rest.Controllers;

[Route("api/devices")]
public class DevicesController : BaseController
{
    private readonly IMediator _mediator;

    public DevicesController(IMediator mediator)
        => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetDevicesFilter filter)
        => HandleResult(await _mediator.SendAsync<GetDevicesQuery, Result<PagedResult<DeviceResponse>>>(
            new GetDevicesQuery(
                filter.Page,
                filter.PageSize,
                filter.Type,
                filter.AdminStatus,
                filter.ManufacturerId)));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
        => HandleResult(await _mediator.SendAsync<GetDeviceByIdQuery, Result<DeviceResponse>>(
            new GetDeviceByIdQuery(id)));

  

   
}