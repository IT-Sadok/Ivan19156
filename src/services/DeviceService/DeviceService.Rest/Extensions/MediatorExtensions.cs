using DeviceService.Application.Commands.AcknowledgeAlert;
using DeviceService.Application.Commands.CreateDevice;
using DeviceService.Application.Commands.CreateRule;
using DeviceService.Application.Commands.DeleteDevice;
using DeviceService.Application.Commands.GenerateApiKey;
using DeviceService.Application.Commands.IssueDeviceCommand;
using DeviceService.Application.Commands.UpdateDevice;
using DeviceService.Application.Queries.GetAlerts;
using DeviceService.Application.Queries.GetCommandTypeBySlug;
using DeviceService.Application.Queries.GetDeviceApiKeys;
using DeviceService.Application.Queries.GetDeviceById;
using DeviceService.Application.Queries.GetDeviceCommands;
using DeviceService.Application.Queries.GetDevices;
using DeviceService.Application.Queries.GetRules;
using IoT.Contracts.Alerts;
using IoT.Contracts.DeviceCommands;
using IoT.Contracts.Devices;
using IoT.Shared.Common;
using IoT.Shared.Mediator;

namespace DeviceService.Rest.Extensions;

public static class MediatorExtensions
{
    public static IServiceCollection AddMediator(this IServiceCollection services)
    {
        services.AddSingleton<IMediator, Mediator>();

        services.AddScoped<IRequestHandler<CreateDeviceCommand, Result<Guid>>, CreateDeviceCommandHandler>();
        services.AddScoped<IRequestHandler<UpdateDeviceCommand, Result<bool>>, UpdateDeviceCommandHandler>();
        services.AddScoped<IRequestHandler<DeleteDeviceCommand, Result<bool>>, DeleteDeviceCommandHandler>();
        services.AddScoped<IRequestHandler<GenerateApiKeyCommand, Result<string>>, GenerateApiKeyCommandHandler>();
        services.AddScoped<IRequestHandler<IssueDeviceCommandCommand, Result<Guid>>, IssueDeviceCommandCommandHandler>();
        services.AddScoped<IRequestHandler<CreateRuleCommand, Result<Guid>>, CreateRuleCommandHandler>();
        services.AddScoped<IRequestHandler<AcknowledgeAlertCommand, Result<bool>>, AcknowledgeAlertCommandHandler>();

        services.AddScoped<IRequestHandler<GetDevicesQuery, Result<PagedResult<DeviceResponse>>>, GetDevicesQueryHandler>();
        services.AddScoped<IRequestHandler<GetDeviceByIdQuery, Result<DeviceResponse>>, GetDeviceByIdQueryHandler>();
        services.AddScoped<IRequestHandler<GetDeviceCommandsQuery, Result<IEnumerable<DeviceCommandResponse>>>, GetDeviceCommandsQueryHandler>();
        services.AddScoped<IRequestHandler<GetDeviceApiKeysQuery, Result<IEnumerable<ApiKeyDto>>>, GetDeviceApiKeysQueryHandler>();
        services.AddScoped<IRequestHandler<GetRulesQuery, Result<IEnumerable<RuleResponse>>>, GetRulesQueryHandler>();
        services.AddScoped<IRequestHandler<GetAlertsQuery, Result<IEnumerable<AlertResponse>>>, GetAlertsQueryHandler>();
        services.AddScoped<IRequestHandler<GetCommandTypeBySlugQuery, Result<Guid>>, GetCommandTypeBySlugQueryHandler>();

        return services;
    }
}
