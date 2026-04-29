// Login/LoginCommandHandler.cs
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IoT.Application.DTOs;
using IoT.Domain.Entities;
using IoT.Interfaces.Mediator;
using IoT.Shared.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace IoT.Application.Identity.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<AuthResponse>>
{
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _config;

    public LoginCommandHandler(UserManager<User> userManager, IConfiguration config)
    {
        _userManager = userManager;
        _config = config;
    }

    public async Task<Result<AuthResponse>> Handle(
        LoginCommand request,
        CancellationToken ct = default)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
            return Result<AuthResponse>.Unauthorized("Invalid credentials");

        var token = GenerateJwt(user);
        return Result<AuthResponse>.Success(token);
    }

    private AuthResponse GenerateJwt(User user)
    {
        var secret = _config["Jwt:Secret"]!;
        var issuer = _config["Jwt:Issuer"]!;
        var audience = _config["Jwt:Audience"]!;
        var expires = DateTime.UtcNow.AddMinutes(
            double.Parse(_config["Jwt:ExpiresInMinutes"]!));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Name, user.UserName!)
            },
            expires: expires,
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        return new AuthResponse(
            new JwtSecurityTokenHandler().WriteToken(token),
            expires);
    }
}