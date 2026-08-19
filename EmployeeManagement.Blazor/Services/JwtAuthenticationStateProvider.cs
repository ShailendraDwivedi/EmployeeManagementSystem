using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace EmployeeManagement.Blazor.Services;

public class JwtAuthenticationStateProvider
    : AuthenticationStateProvider
{
    private readonly ITokenStorageService _tokenStorage;

    private static readonly ClaimsPrincipal Anonymous =
        new(new ClaimsIdentity());

    private bool _initialized;

    public JwtAuthenticationStateProvider(
        ITokenStorageService tokenStorage)
    {
        _tokenStorage = tokenStorage;
    }

    public override async Task<AuthenticationState>
        GetAuthenticationStateAsync()
    {
        if (!_initialized)
        {
            return new AuthenticationState(
                Anonymous);
        }

        try
        {
            var token =
                await _tokenStorage.GetTokenAsync();

            if (string.IsNullOrWhiteSpace(token))
            {
                return new AuthenticationState(
                    Anonymous);
            }

            var identity =
                CreateIdentityFromJwt(token);

            return new AuthenticationState(
                new ClaimsPrincipal(identity));
        }
        catch
        {
            return new AuthenticationState(
                Anonymous);
        }
    }

    private static ClaimsIdentity
        CreateIdentityFromJwt(string token)
    {
        var handler =
            new JwtSecurityTokenHandler();

        var jwtToken =
            handler.ReadJwtToken(token);

        var claims =
            jwtToken.Claims.ToList();

        return new ClaimsIdentity(
            claims,
            authenticationType: "jwt",
            nameType: ClaimTypes.Name,
            roleType: ClaimTypes.Role);
    }

    public async Task InitializeAsync()
    {
        if (_initialized)
        {
            return;
        }

        _initialized = true;

        var token =
            await _tokenStorage.GetTokenAsync();

        if (string.IsNullOrWhiteSpace(token))
        {
            NotifyAuthenticationStateChanged(
                Task.FromResult(
                    new AuthenticationState(
                        Anonymous)));

            return;
        }

        var identity =
            CreateIdentityFromJwt(token);

        var user =
            new ClaimsPrincipal(identity);

        NotifyAuthenticationStateChanged(
            Task.FromResult(
                new AuthenticationState(user)));
    }

    public void NotifyUserAuthentication(
        string token)
    {
        _initialized = true;

        var identity =
            CreateIdentityFromJwt(token);

        var user =
            new ClaimsPrincipal(identity);

        NotifyAuthenticationStateChanged(
            Task.FromResult(
                new AuthenticationState(user)));
    }

    public void NotifyUserLogout()
    {
        _initialized = true;

        NotifyAuthenticationStateChanged(
            Task.FromResult(
                new AuthenticationState(
                    Anonymous)));
    }
}