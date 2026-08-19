using EmployeeManagement.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EmployeeManagement.Infrastructure.Services;

public class JwtTokenService : IJwtTokenService
{
    private readonly IConfiguration _configuration;

    public JwtTokenService(
        IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task<string> GenerateTokenAsync(
        Guid userId,
        string email,
        string userName,
        IEnumerable<string> roles)
    {
        var jwtSettings =
            _configuration.GetSection("JwtSettings");

        var key =
            jwtSettings["SecretKey"]
            ?? throw new InvalidOperationException(
                "JWT Secret Key is not configured.");

        var issuer =
            jwtSettings["Issuer"];

        var audience =
            jwtSettings["Audience"];

        var expiryMinutes =
            int.TryParse(
                jwtSettings["ExpiryMinutes"],
                out var minutes)
                ? minutes
                : 30;

        var claims = new List<Claim>
        {
            new Claim(
                JwtRegisteredClaimNames.Sub,
                userId.ToString()),

            new Claim(    
                ClaimTypes.NameIdentifier,
                userId.ToString()),

            new Claim(
                JwtRegisteredClaimNames.Email,
                email),

            new Claim(
                JwtRegisteredClaimNames.UniqueName,
                userName),

            new Claim(
                ClaimTypes.NameIdentifier,
                userId.ToString()),

            new Claim(
                ClaimTypes.Email,
                email),

            new Claim(
                ClaimTypes.Name,
                userName)
        };

        foreach (var role in roles)
        {
            claims.Add(
                new Claim(
                    ClaimTypes.Role,
                    role));
        }

        var securityKey =
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(key));

        var credentials =
            new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256);

        var token =
            new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    expiryMinutes),
                signingCredentials: credentials);

        var accessToken =
            new JwtSecurityTokenHandler()
                .WriteToken(token);

        return Task.FromResult(accessToken);
    }
}