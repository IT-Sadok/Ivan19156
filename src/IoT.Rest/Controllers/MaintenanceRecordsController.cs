using IoT.Application.Commands.Maintenance.CreateMaintenanceRecord;
using IoT.Application.Common.Mappings;
using IoT.Application.Queries.Maintenance.GetMaintenanceRecords;
using IoT.Contracts.Maintenance;
using IoT.Shared.Common;
using IoT.Shared.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace IoT.Rest.Controllers;

//[Authorize]
[Route("api/devices/{deviceId:guid}/maintenance")]
public class MaintenanceRecordsController(IMediator mediator) : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetAll(Guid deviceId, CancellationToken ct)
        => HandleResult(await mediator.SendAsync<GetMaintenanceRecordsQuery, Result<IEnumerable<MaintenanceRecordResponse>>>(
            new GetMaintenanceRecordsQuery(deviceId), ct));

    [HttpPost]
    public async Task<IActionResult> Create(
        Guid deviceId,
        [FromBody] CreateMaintenanceRecordRequest request,
        CancellationToken ct)
        => HandleResult(await mediator.SendAsync<CreateMaintenanceRecordCommand, Result<MaintenanceRecordResponse>>(
            request.ToCommand(deviceId), ct));
}