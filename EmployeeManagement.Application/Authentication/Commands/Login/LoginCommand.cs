using EmployeeManagement.Application.Authentication.Responses;
using MediatR;

namespace EmployeeManagement.Application.Authentication.Commands.Login;

public class LoginCommand : IRequest<AuthResponse>
{
    public string UserName { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}