using IoT.Shared.Common;
using IoT.Shared.Mediator;

namespace DeviceService.Application.Queries.GetCommandTypeBySlug;

public record GetCommandTypeBySlugQuery(string Slug) : IRequest<Result<Guid>>;