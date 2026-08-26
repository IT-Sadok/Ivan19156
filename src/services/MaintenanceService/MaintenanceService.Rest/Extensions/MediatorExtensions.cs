using IoT.Contracts.Maintenance;
using IoT.Shared.Common;
using IoT.Shared.Mediator;
using MaintenanceService.Application.Commands.CreateMaintenanceRecord;
using MaintenanceService.Application.Queries.GetMaintenanceRecords;

namespace MaintenanceService.Rest.Extensions;

public static class MediatorExtensions
{
    public static IServiceCollection AddMediator(this IServiceCollection services)
    {
        services.AddSingleton<IMediator, Mediator>();
        services.AddScoped<IRequestHandler<CreateMaintenanceRecordCommand, Result<MaintenanceRecordResponse>>, CreateMaintenanceRecordCommandHandler>();
        services.AddScoped<IRequestHandler<GetMaintenanceRecordsQuery, Result<IEnumerable<MaintenanceRecordResponse>>>, GetMaintenanceRecordsQueryHandler>();
        return services;
    }
}