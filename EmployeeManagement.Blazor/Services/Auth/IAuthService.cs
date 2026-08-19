using EmployeeManagement.Blazor.Models.AuthModels;

namespace EmployeeManagement.Blazor.Services.Auth
{
    public interface IAuthService
    {
        Task<AuthResponse> LoginAsync(LoginRequest request);
        Task<string?> GetTokenAsync();
        Task LogoutAsync();
    }
}
