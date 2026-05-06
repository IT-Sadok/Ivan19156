using IoT.Domain.Entities;
using IoT.Interfaces.Mediator;
using IoT.Shared.Common;
using Microsoft.AspNetCore.Identity;

namespace IoT.Application.Commands.Identity.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<string>>
{
    private readonly UserManager<User> _userManager;

    public RegisterCommandHandler(UserManager<User> userManager)
        => _userManager = userManager;

    public async Task<Result<string>> Handle(
        RegisterCommand request,
        CancellationToken ct = default)
    {
        var user = new User
        {
            UserName = request.UserName,
            Email = request.Email
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return Result<string>.Failure(errors);
        }

        return Result<string>.Success("User registered successfully");
    }
}
