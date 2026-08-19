namespace EmployeeManagement.Blazor.Models.AuthModels
{
    public class AuthResponse
    {
        public bool Success { get; set; }

        public string Message { get; set; } = string.Empty;

        public AuthData? Data { get; set; }
    }
    public class AuthData
    {
        public string AccessToken { get; set; } = string.Empty;

        public string RefreshToken { get; set; } = string.Empty;
    }
}
