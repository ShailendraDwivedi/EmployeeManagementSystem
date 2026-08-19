using EmployeeManagement.Application.Authentication.DTOs;

namespace EmployeeManagement.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<JwtUserDto?> FindByUserNameAsync(
        string userName);

    Task<JwtUserDto?> FindByEmailAsync(
        string email);

    Task<(bool Success, string[] Errors, string? UserId)>
        CreateUserAsync(
            string userName,
            string email,
            string password,
            string firstName,
            string lastName);

    Task<bool> CheckPasswordAsync(
        string userName,
        string password);

    Task<IList<string>> GetRolesAsync(
        string userId);

    Task<bool> AddToRoleAsync(
        string userId,
        string role);

    Task<JwtUserDto?> FindByIdAsync(
    string userId);
}