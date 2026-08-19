using System.Security.Claims;

namespace EmployeeManagement.Application.Common.Interfaces;

public interface IJwtTokenService
{
    Task<string> GenerateTokenAsync(
        Guid userId,
        string email,
        string userName,
        IEnumerable<string> roles);
}