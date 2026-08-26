using IoT.Contracts.Maintenance;
using IoT.Shared.Common;
using IoT.Shared.Mediator;
using MaintenanceService.Application.Commands.CreateMaintenanceRecord;
using MaintenanceService.Application.Queries.GetMaintenanceRecords;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaintenanceService.Rest.Controllers;

[Route("api/devices/{deviceId:guid}/maintenance")]
[Authorize]
public class MaintenanceRecordsController : BaseController
{
    private readonly IMediator _mediator;

    public MaintenanceRecordsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll(Guid deviceId)
    {
        var query = new GetMaintenanceRecordsQuery(deviceId);
        return HandleResult(await _mediator.SendAsync<GetMaintenanceRecordsQuery, Result<IEnumerable<MaintenanceRecordResponse>>>(query));
    }

    [HttpPost]
    public async Task<IActionResult> Create(Guid deviceId, [FromBody] CreateMaintenanceRecordRequest request)
    {
        var command = new CreateMaintenanceRecordCommand(
            deviceId,
            request.TechnicianId,
            request.Notes,
            request.PerformedAt);
        return HandleResult(await _mediator.SendAsync<CreateMaintenanceRecordCommand, Result<MaintenanceRecordResponse>>(command));
    }
}