// IoT.Application/Commands/Devices/GenerateApiKey/GenerateApiKeyCommandHandler.cs
using IoT.Interfaces;
using IoT.Shared.Mediator;
using IoT.Interfaces.Services;
using IoT.Shared.Common;

namespace IoT.Application.Commands.Devices.GenerateApiKey;

public class GenerateApiKeyCommandHandler
    : IRequestHandler<GenerateApiKeyCommand, Result<string>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IApiKeyService _apiKeyService;

    public GenerateApiKeyCommandHandler(
        IUnitOfWork unitOfWork,
        IApiKeyService apiKeyService)
    {
        _unitOfWork = unitOfWork;
        _apiKeyService = apiKeyService;
    }

    public async Task<Result<string>> ExecuteAsync(
        GenerateApiKeyCommand request,
        CancellationToken ct = default)
    {
        var device = await _unitOfWork.Devices.GetByIdAsync(request.DeviceId, ct);

        if (device == null)
            return Result<string>.NotFound($"Device {request.DeviceId} not found");

        var apiKey = await _apiKeyService.GenerateAsync(request.DeviceId, ct);

        return Result<string>.Success(apiKey);
    }
}