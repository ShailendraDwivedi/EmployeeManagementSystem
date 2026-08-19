using EmployeeManagement.Application.Authentication.Responses;
using MediatR;

namespace EmployeeManagement.Application.Authentication.Commands.RefreshToken;

public class RefreshTokenCommand : IRequest<AuthResponse>
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}