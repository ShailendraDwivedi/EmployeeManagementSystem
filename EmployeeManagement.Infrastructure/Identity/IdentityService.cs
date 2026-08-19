using EmployeeManagement.Application.Authentication.DTOs;
using EmployeeManagement.Application.Common.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace EmployeeManagement.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser>
        _userManager;

    public IdentityService(
        UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<JwtUserDto?> FindByUserNameAsync(
        string userName)
    {
        var user =
            await _userManager.FindByNameAsync(
                userName);

        if (user == null)
            return null;

        return await MapUserAsync(user);
    }

    public async Task<JwtUserDto?> FindByEmailAsync(
        string email)
    {
        var user =
            await _userManager.FindByEmailAsync(
                email);

        if (user == null)
            return null;

        return await MapUserAsync(user);
    }
    public async Task<JwtUserDto?> FindByIdAsync(string userId)
    {
        var user =
            await _userManager
                .FindByIdAsync(userId);

        if (user == null)
        {
            return null;
        }

        return new JwtUserDto
        {
            Id =
                user.Id,

            UserName =
                user.UserName ?? string.Empty,

            Email =
                user.Email ?? string.Empty,

            FirstName =
                user.FirstName,

            LastName =
                user.LastName
        };
    }
    public async Task<(bool Success, string[] Errors, string? UserId)>
        CreateUserAsync(
            string userName,
            string email,
            string password,
            string firstName,
            string lastName)
    {
        var user = new ApplicationUser
        {
            Id = Guid.NewGuid().ToString(),

            UserName = userName,

            Email = email,

            FirstName = firstName,

            LastName = lastName,

            EmailConfirmed = true
        };

        var result =
            await _userManager.CreateAsync(
                user,
                password);

        if (!result.Succeeded)
        {
            return (
                false,
                result.Errors
                    .Select(x => x.Description)
                    .ToArray(), null);
        }

        return (true, Array.Empty<string>(), user.Id);
    }

    public async Task<bool> CheckPasswordAsync(
        string userName,
        string password)
    {
        var user =
            await _userManager.FindByNameAsync(
                userName);

        if (user == null)
            return false;

        return await _userManager.CheckPasswordAsync(
            user,
            password);
    }

    public async Task<IList<string>> GetRolesAsync(
        string userId)
    {
        var user =
            await _userManager.FindByIdAsync(
                userId);

        if (user == null)
            return new List<string>();

        return await _userManager.GetRolesAsync(user);
    }

    public async Task<bool> AddToRoleAsync(
        string userId,
        string role)
    {
        var user =
            await _userManager.FindByIdAsync(
                userId);

        if (user == null)
            throw new Exception("User not found.");

        if (await _userManager.IsInRoleAsync(user, role))
            return true;

        var result =
            await _userManager.AddToRoleAsync(
                user,
                role);

        if (!result.Succeeded)
        {
            var errors = string.Join(
                ", ",
                result.Errors.Select(
                    x => x.Description));

            throw new Exception(
                $"Failed to add user to role '{role}': {errors}");
        }

        return true;
    }

    private async Task<JwtUserDto> MapUserAsync(
        ApplicationUser user)
    {
        var roles =
            await _userManager.GetRolesAsync(user);

        return new JwtUserDto
        {
            Id = user.Id,

            UserName =
                user.UserName ?? string.Empty,

            Email =
                user.Email ?? string.Empty,

            Roles = roles.ToList()
        };
    }
}