using EmployeeManagement.Blazor.Models;
using EmployeeManagement.Blazor.Models.DesignationModels;
using System.Net.Http.Headers;

namespace EmployeeManagement.Blazor.Services.Designation;

public class DesignationService : IDesignationService
{
    private readonly HttpClient _httpClient;
    private readonly ITokenStorageService _tokenStorage;

    public DesignationService(
        IHttpClientFactory httpClientFactory,
        ITokenStorageService tokenStorage)
    {
        _httpClient =
            httpClientFactory.CreateClient("EmployeeAPI");

        _tokenStorage = tokenStorage;
    }

    public async Task<PagedResult<DesignationDto>> GetDesignationsAsync(
            int pageNumber = 1,
            int pageSize = 10,
            string? search = null)
    {
        var token =
            await _tokenStorage.GetTokenAsync();

        if (string.IsNullOrWhiteSpace(token))
        {
            throw new Exception(
                "JWT token is not available.");
        }

        var url =
            $"api/Designations?pageNumber={pageNumber}" +
            $"&pageSize={pageSize}";

        if (!string.IsNullOrWhiteSpace(search))
        {
            url +=
                $"&search={Uri.EscapeDataString(search)}";
        }

        using var request =
            new HttpRequestMessage(
                HttpMethod.Get,
                url);

        request.Headers.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                token);

        var response =
            await _httpClient.SendAsync(request);

        var responseBody =
            await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(
                $"Designation API error: " +
                $"{(int)response.StatusCode} " +
                $"{response.StatusCode}. " +
                $"{responseBody}");
        }

        var result =
            System.Text.Json.JsonSerializer.Deserialize<
                ApiResponse<PagedResult<DesignationDto>>>(
                responseBody,
                new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

        return result?.Data
            ?? new PagedResult<DesignationDto>();
    }

    public async Task<DesignationDto?> GetDesignationByIdAsync(Guid id)
    {
        var token =
            await _tokenStorage.GetTokenAsync();

        if (string.IsNullOrWhiteSpace(token))
        {
            throw new Exception(
                "JWT token is not available.");
        }

        using var request =
            new HttpRequestMessage(
                HttpMethod.Get,
                $"api/Designations/{id}");

        request.Headers.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                token);

        var response =
            await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var result =
            await response.Content
                .ReadFromJsonAsync<
                    ApiResponse<DesignationDto>>();

        return result?.Data;
    }

    public async Task<bool> CreateDesignationAsync(
            CreateDesignationRequest request)
    {
        var token =
            await _tokenStorage.GetTokenAsync();

        if (string.IsNullOrWhiteSpace(token))
        {
            throw new Exception(
                "JWT token is not available.");
        }

        using var httpRequest =
            new HttpRequestMessage(
                HttpMethod.Post,
                "api/Designations");

        httpRequest.Headers.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                token);

        httpRequest.Content =
            JsonContent.Create(request);

        var response =
            await _httpClient.SendAsync(
                httpRequest);

        //var message =
        //    await response.Content
        //        .ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            var message = await GetApiErrorMessage(response);

            throw new HttpRequestException(message);
        }

        return true;
    }

    public async Task<bool> UpdateDesignationAsync(Guid id, UpdateDesignationRequest request)
    {
        var token =
            await _tokenStorage.GetTokenAsync();

        if (string.IsNullOrWhiteSpace(token))
        {
            throw new Exception(
                "JWT token is not available.");
        }

        using var httpRequest =
            new HttpRequestMessage(
                HttpMethod.Put,
                $"api/Designations/{id}");

        httpRequest.Headers.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                token);

        httpRequest.Content =
            JsonContent.Create(request);

        var response =
            await _httpClient.SendAsync(
                httpRequest);

        //var message =
        //    await response.Content
        //        .ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            var message = await GetApiErrorMessage(response);

            throw new HttpRequestException(message);
        }

        return true;
    }

    public async Task<bool> DeleteDesignationAsync(Guid id)
    {
        var token =
            await _tokenStorage.GetTokenAsync();

        if (string.IsNullOrWhiteSpace(token))
        {
            throw new Exception(
                "JWT token is not available.");
        }

        using var request =
            new HttpRequestMessage(
                HttpMethod.Delete,
                $"api/Designations/{id}");

        request.Headers.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                token);

        var response =
            await _httpClient.SendAsync(request);

        var message =
            await response.Content
                .ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(
                string.IsNullOrWhiteSpace(message)
                    ? $"Unable to delete designation. " +
                      $"Status: {response.StatusCode}"
                    : message);
        }

        return true;
    }

    // PUT THE HELPER HERE
    private static async Task<string> GetApiErrorMessage(
        HttpResponseMessage response)
    {
        var body =
            await response.Content.ReadAsStringAsync();

        if (string.IsNullOrWhiteSpace(body))
        {
            return $"Request failed. Status: {response.StatusCode}";
        }

        try
        {
            var result =
                System.Text.Json.JsonSerializer
                    .Deserialize<ApiErrorResponse>(
                        body,
                        new System.Text.Json.JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

            if (result?.Errors != null &&
                result.Errors.Count > 0)
            {
                var messages =
                    result.Errors
                        .SelectMany(x => x.Value);

                return string.Join(" ", messages);
            }

            if (!string.IsNullOrWhiteSpace(result?.Message))
            {
                return result.Message;
            }
        }
        catch
        {
            // Fall back to API response
        }

        return body;
    }
}