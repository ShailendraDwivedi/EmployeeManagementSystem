using EmployeeManagement.Application.Authentication.Responses;
using MediatR;

namespace EmployeeManagement.Application.Authentication.Commands.Register;

public record RegisterCommand(
    string UserName,
    string Email,
    string Password,
    string FirstName,
    string LastName
) : IRequest<string>;