using IoT.Application.Commands.Identity.Login;
using IoT.Application.Commands.Identity.Register;
using IoT.Contracts.Identity;

namespace IoT.Application.Common.Mappings;

public static class IdentityMappingExtensions
{
    public static RegisterCommand ToCommand(this RegisterRequest request)
        => new(request.UserName, request.Email, request.Password, request.Role);

    public static LoginCommand ToCommand(this LoginRequest request)
        => new(request.Email, request.Password);
}
