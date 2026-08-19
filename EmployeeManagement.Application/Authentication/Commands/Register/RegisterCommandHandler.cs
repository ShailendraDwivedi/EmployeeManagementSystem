using EmployeeManagement.Application.Common.Interfaces;
using MediatR;

namespace EmployeeManagement.Application.Authentication.Commands.Register;

public class RegisterCommandHandler
    : IRequestHandler<RegisterCommand, string>
{
    private readonly IIdentityService _identityService;

    public RegisterCommandHandler(
        IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<string> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken)
    {
        // 1. Check username
        var existingUser =
            await _identityService.FindByUserNameAsync(
                request.UserName);

        if (existingUser != null)
        {
            throw new Exception(
                "Username already exists.");
        }

        // 2. Check email
        var existingEmail =
            await _identityService.FindByEmailAsync(
                request.Email);

        if (existingEmail != null)
        {
            throw new Exception(
                "Email already exists.");
        }

        // 3. Create user
        var result =
            await _identityService.CreateUserAsync(
                request.UserName,
                request.Email,
                request.Password,
                request.FirstName,
                request.LastName);

        if (!result.Success)
        {
            var errors =
                string.Join(", ", result.Errors);

            throw new Exception(errors);
        }

        var userId = result.UserId!;

        // 4. Add default EMPLOYEE role
        var roleResult =
            await _identityService.AddToRoleAsync(
                userId,
                "EMPLOYEE");

        if (!roleResult)
        {
            throw new Exception(
                "Failed to add user to role EMPLOYEE.");
        }

        // 5. Registration completed
        return $"{userId} User registered successfully.";
    }
}