using Microsoft.JSInterop;

namespace EmployeeManagement.Blazor.Services
{
    public class TokenStorageService : ITokenStorageService
    {
        private const string TokenKey = "access_token";

        private readonly IJSRuntime _jsRuntime;

        public TokenStorageService(
            IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task SetTokenAsync(
            string token)
        {
            await _jsRuntime.InvokeVoidAsync(
                "localStorage.setItem",
                TokenKey,
                token);
        }

        public async Task<string?> GetTokenAsync()
        {
            return await _jsRuntime.InvokeAsync<string?>(
                "localStorage.getItem",
                TokenKey);
        }

        public async Task RemoveTokenAsync()
        {
            await _jsRuntime.InvokeVoidAsync(
                "localStorage.removeItem",
                TokenKey);
        }
    }
}
