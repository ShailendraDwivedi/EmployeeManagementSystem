using EmployeeManagement.Application.Authentication.DTOs;
using EmployeeManagement.Application.Authentication.Responses;

namespace EmployeeManagement.Application.Authentication.Interfaces;

public interface IJwtTokenGenerator
{
    Task<AuthResponse> GenerateAsync(JwtUserDto user);
}