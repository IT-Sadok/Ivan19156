using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace DeviceService.IntegrationTests.Infrastructure;
public static class JwtTokenHelper
{
    public static string GenerateToken(
        Guid userId,
        string role = "Admin",
        string secret = TestConstants.Jwt.Secret)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Role, role),
            new Claim(ClaimTypes.Email, "test@test.com")
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        key.KeyId = "dev-key";
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: TestConstants.Jwt.Issuer,
            audience: TestConstants.Jwt.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    public static void SetBearerToken(this HttpClient client, Guid userId, string role = "Admin")
    {
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", GenerateToken(userId, role));
    }
}