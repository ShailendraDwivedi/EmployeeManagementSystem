using EmployeeManagement.Application.Authentication.DTOs;
using EmployeeManagement.Application.Authentication.Interfaces;
using EmployeeManagement.Application.Authentication.Responses;
using EmployeeManagement.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EmployeeManagement.Infrastructure.Authentication;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtOptions _options;

    public JwtTokenGenerator(IOptions<JwtOptions> options, UserManager<ApplicationUser> userManager)
    {
        _options = options.Value;
    }

    public Task<AuthResponse> GenerateAsync(JwtUserDto user)
    {
        var claims = new List<Claim>
        {
            new( JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new(ClaimTypes.Name,user.UserName ?? string.Empty)
        };

        // Add roles to JWT
        foreach (var role in user.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiry = DateTime.UtcNow.AddMinutes(_options.ExpiryMinutes);

        var token = new JwtSecurityToken
            (
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                expires: expiry,
                signingCredentials: credentials
            );

        return Task.FromResult(new AuthResponse
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = expiry,
            RefreshToken = string.Empty
        });
    }
}