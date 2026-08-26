using DeviceService.Interfaces;
using IoT.Shared.Common;
using IoT.Shared.Mediator;

namespace DeviceService.Application.Queries.GetCommandTypeBySlug;

public class GetCommandTypeBySlugQueryHandler : IRequestHandler<GetCommandTypeBySlugQuery, Result<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetCommandTypeBySlugQueryHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<Result<Guid>> ExecuteAsync(GetCommandTypeBySlugQuery request, CancellationToken ct = default)
    {
        var commandType = await _unitOfWork.CommandTypes.GetBySlugAsync(request.Slug, ct);
        if (commandType is null)
            return Result<Guid>.NotFound($"Command type '{request.Slug}' not found.");

        return Result<Guid>.Success(commandType.Id);
    }
}