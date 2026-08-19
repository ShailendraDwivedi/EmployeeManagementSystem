using EmployeeManagement.Blazor.Models.AuthModels;

namespace EmployeeManagement.Blazor.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenStorageService _tokenStorage;

        public AuthService(IHttpClientFactory httpClientFactory, ITokenStorageService tokenStorage)
        {
            _httpClient = httpClientFactory.CreateClient("API");
            _tokenStorage = tokenStorage;
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Auth/login", request);

            if (!response.IsSuccessStatusCode)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = $"Login failed: " + $"{(int)response.StatusCode}"
                };
            }

            var result = await response.Content.ReadFromJsonAsync<AuthResponse>();

            return result ?? new AuthResponse
            {
                Success = false,
                Message = "Invalid response received from API."
            };
        }

        public async Task LogoutAsync()
        {
            await _tokenStorage.RemoveTokenAsync();
        }

        public Task<string?> GetTokenAsync()
        {
            return _tokenStorage.GetTokenAsync();
        }
    }
}