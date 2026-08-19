using System.Net.Http.Headers;

namespace EmployeeManagement.Blazor.Services;

public class JwtAuthorizationHandler : DelegatingHandler
{
    private readonly ITokenStorageService _tokenStorage;

    public JwtAuthorizationHandler(
        ITokenStorageService tokenStorage)
    {
        _tokenStorage = tokenStorage;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        string? token = null;

        try
        {
            token = await _tokenStorage.GetTokenAsync();
            Console.WriteLine(
    $"JWT FOUND: {!string.IsNullOrWhiteSpace(token)}");
        }
        catch (InvalidOperationException)
        {
            // JS interop isn't available during prerendering.
            // Continue without JWT.
        }

        if (!string.IsNullOrWhiteSpace(token))
        {
            request.Headers.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    token);
            Console.WriteLine(
        $"JWT ADDED: {request.Method} {request.RequestUri}");
        }

        return await base.SendAsync(
            request,
            cancellationToken);

    }
}