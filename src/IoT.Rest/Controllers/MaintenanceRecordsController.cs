// IoT.Rest/Controllers/MaintenanceRecordsController.cs

using IoT.Application.Common.Mappings;
using IoT.Application.Queries.Maintenance.GetMaintenanceRecords;
using IoT.Contracts.Maintenance;
using IoT.Domain.Constants;
using IoT.Shared.Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IoT.Rest.Controllers;

//[Authorize]
[Route("api/devices/{deviceId:guid}/maintenance")]
public class MaintenanceRecordsController(IMediator mediator) : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetAll(Guid deviceId, CancellationToken ct)
        => HandleResult(await mediator.Send(new GetMaintenanceRecordsQuery(deviceId), ct));

    //[Authorize(Roles = Roles.Admin)]
    [HttpPost]
    public async Task<IActionResult> Create(
        Guid deviceId,
        [FromBody] CreateMaintenanceRecordRequest request,
        CancellationToken ct)
        => HandleResult(await mediator.Send(request.ToCommand(deviceId), ct));
}