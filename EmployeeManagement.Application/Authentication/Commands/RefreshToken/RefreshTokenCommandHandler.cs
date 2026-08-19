using EmployeeManagement.Application.Authentication.Responses;
using EmployeeManagement.Application.Common.Interfaces;
using MediatR;
using System.Security.Cryptography;

namespace EmployeeManagement.Application.Authentication.Commands.RefreshToken;

public class RefreshTokenCommandHandler
    : IRequestHandler<RefreshTokenCommand, AuthResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IIdentityService _identityService;
    private readonly IJwtTokenService _jwtTokenService;

    public RefreshTokenCommandHandler(
        IUnitOfWork unitOfWork,
        IIdentityService identityService,
        IJwtTokenService jwtTokenService)
    {
        _unitOfWork = unitOfWork;
        _identityService = identityService;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<AuthResponse> Handle(
        RefreshTokenCommand request,
        CancellationToken cancellationToken)
    {
        // 1. Validate refresh token input
        if (string.IsNullOrWhiteSpace(request.RefreshToken))
        {
            throw new UnauthorizedAccessException(
                "Refresh token is required.");
        }

        // 2. Find refresh token
        var refreshTokens =
            await _unitOfWork.RefreshTokens.FindAsync(
                x => x.Token == request.RefreshToken,
                cancellationToken);

        var refreshToken =
            refreshTokens.FirstOrDefault();

        if (refreshToken == null)
        {
            throw new UnauthorizedAccessException(
                "Invalid refresh token.");
        }

        // 3. Check expiration/revocation
        if (!refreshToken.IsActive)
        {
            throw new UnauthorizedAccessException(
                "Refresh token is expired or revoked.");
        }

        // 4. Find user
        var user =
            await _identityService.FindByIdAsync(
                refreshToken.UserId);

        if (user == null)
        {
            throw new UnauthorizedAccessException(
                "User not found.");
        }

        // 5. Get current roles
        var roles =
            await _identityService.GetRolesAsync(
                user.Id);

        // 6. Convert string Identity ID to Guid
        if (!Guid.TryParse(user.Id, out var userId))
        {
            throw new InvalidOperationException(
                "Invalid user ID.");
        }

        // 7. Generate NEW access token
        var accessToken =
            await _jwtTokenService.GenerateTokenAsync(
                userId,
                user.Email,
                user.UserName,
                roles);

        // 8. Revoke OLD refresh token
        refreshToken.RevokedAt =
            DateTime.UtcNow;

        // 9. Generate NEW refresh token
        var newRefreshToken =
            GenerateRefreshToken();

        // 10. Create new refresh-token entity
        var newRefreshTokenEntity =
            new Domain.Entities.RefreshToken
            {
                Id = Guid.NewGuid(),

                Token = newRefreshToken,

                UserId = user.Id,

                CreatedAt = DateTime.UtcNow,

                ExpiresAt =
                    DateTime.UtcNow.AddDays(7),

                RevokedAt = null
            };

        // 11. Save new refresh token
        await _unitOfWork.RefreshTokens.AddAsync(
            newRefreshTokenEntity,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        // 12. Return new tokens
        return new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken
        };
    }

    private static string GenerateRefreshToken()
    {
        var bytes =
            RandomNumberGenerator.GetBytes(64);

        return Convert.ToBase64String(bytes);
    }
}