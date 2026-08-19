using EmployeeManagement.Application.Authentication.Responses;
using EmployeeManagement.Application.Common.Interfaces;
using MediatR;
using System.Security.Cryptography;

using RefreshTokenEntity =
    EmployeeManagement.Domain.Entities.RefreshToken;

namespace EmployeeManagement.Application.Authentication.Commands.Login;

public class LoginCommandHandler
    : IRequestHandler<LoginCommand, AuthResponse>
{
    private readonly IIdentityService _identityService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtTokenService _jwtTokenService;

    public LoginCommandHandler(
        IIdentityService identityService,
        IUnitOfWork unitOfWork,
        IJwtTokenService jwtTokenService)
    {
        _identityService = identityService;
        _unitOfWork = unitOfWork;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<AuthResponse> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        // 1. Find user
        var user =  
            await _identityService.FindByUserNameAsync(
                request.UserName);

        if (user == null)
        {
            throw new UnauthorizedAccessException(
                "Invalid username or password.");
        }

        // 2. Validate password
        var passwordValid =
            await _identityService.CheckPasswordAsync(
                user.UserName,
                request.Password);

        if (!passwordValid)
        {
            throw new UnauthorizedAccessException(
                "Invalid username or password.");
        }

        // 3. Get user roles
        var roles =
            await _identityService.GetRolesAsync(
                user.Id);
        if (!Guid.TryParse(user.Id, out var userId))
        {
            throw new InvalidOperationException(
                "Invalid user ID.");
        }

        // 4. Generate JWT
        var accessToken =
            await _jwtTokenService.GenerateTokenAsync(
               userId,
                user.Email,
                user.UserName,
                roles);

        // 5. Generate refresh token
        var refreshToken =
            GenerateRefreshToken();

        // 6. Create refresh-token entity
        var refreshTokenEntity =
            new RefreshTokenEntity
            {
                Id = Guid.NewGuid(),

                Token = refreshToken,

                UserId = user.Id.ToString(),

                CreatedAt = DateTime.UtcNow,

                ExpiresAt =
                    DateTime.UtcNow.AddDays(7),

                RevokedAt = null
            };

        // 7. Save refresh token
        await _unitOfWork.RefreshTokens.AddAsync(
            refreshTokenEntity,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        // 8. Build response
        return new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    private static string GenerateRefreshToken()
    {
        var bytes =
            RandomNumberGenerator.GetBytes(64);

        return Convert.ToBase64String(bytes);
    }
}