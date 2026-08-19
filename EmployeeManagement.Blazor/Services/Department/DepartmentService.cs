using EmployeeManagement.Blazor.Models;
using EmployeeManagement.Blazor.Models.DepartmentModels;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace EmployeeManagement.Blazor.Services.Department;

public class DepartmentService : IDepartmentService
{
    private readonly HttpClient _httpClient;
    private readonly ITokenStorageService _tokenStorage;

    public DepartmentService(
        IHttpClientFactory httpClientFactory,
        ITokenStorageService tokenStorage)
    {
        _httpClient =
            httpClientFactory.CreateClient("EmployeeAPI");

        _tokenStorage = tokenStorage;
    }

    public async Task<PagedResult<DepartmentDto>> GetDepartmentsAsync(
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
            $"api/Departments?pageNumber={pageNumber}" +
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
                $"Department API error: " +
                $"{(int)response.StatusCode} " +
                $"{response.StatusCode}. " +
                $"{responseBody}");
        }

        var result =
            System.Text.Json.JsonSerializer.Deserialize<
                ApiResponse<PagedResult<DepartmentDto>>>(
                responseBody,
                new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

        return result?.Data
            ?? new PagedResult<DepartmentDto>();
    }

    public async Task<DepartmentDto?> GetDepartmentByIdAsync(Guid id)
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
                $"api/Departments/{id}");

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
                    ApiResponse<DepartmentDto>>();

        return result?.Data;
    }

    public async Task<bool> CreateDepartmentAsync(
            CreateDepartmentRequest request)
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
                "api/Departments");

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

    public async Task<bool> UpdateDepartmentAsync(
            Guid id,
            UpdateDepartmentRequest request)
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
                $"api/Departments/{id}");

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

    public async Task<bool>
        DeleteDepartmentAsync(Guid id)
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
                $"api/Departments/{id}");

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
                    ? $"Unable to delete department. " +
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