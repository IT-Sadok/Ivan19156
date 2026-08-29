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

  
}