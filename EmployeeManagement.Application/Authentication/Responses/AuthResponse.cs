namespace EmployeeManagement.Application.Authentication.Responses;

public class AuthResponse
{
    public string AccessToken { get; set; } = string.Empty;

    public DateTime Expiration { get; set; }

    public string RefreshToken { get; set; } = string.Empty;
}