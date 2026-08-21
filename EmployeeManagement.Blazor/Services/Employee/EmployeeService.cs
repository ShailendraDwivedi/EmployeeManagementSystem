using EmployeeManagement.Blazor.Models;
using EmployeeManagement.Blazor.Models.EmployeeModels;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace EmployeeManagement.Blazor.Services.Employee;

public class EmployeeService : IEmployeeService
{
    private readonly HttpClient _httpClient;
    private readonly ITokenStorageService _tokenStorage;

    public EmployeeService(IHttpClientFactory httpClientFactory, ITokenStorageService tokenStorage)
    {
        _httpClient = httpClientFactory.CreateClient("EmployeeAPI");
        _tokenStorage = tokenStorage;
    }

    public async Task<PagedResult<EmployeeDto>> GetEmployeesAsync(
        int pageNumber = 1,
        int pageSize = 10,
        string? search = null)
    {
        // 1. Get JWT from localStorage
        var token =
            await _tokenStorage.GetTokenAsync();

        if (string.IsNullOrWhiteSpace(token))
        {
            throw new Exception(
                "JWT token is not available.");
        }

        // 2. Build API URL
        var url =
            $"api/Employees?pageNumber={pageNumber}" +
            $"&pageSize={pageSize}";

        if (!string.IsNullOrWhiteSpace(search))
        {
            url +=
                $"&search={Uri.EscapeDataString(search)}";
        }

        // 3. Create request
        using var request =
            new HttpRequestMessage(
                HttpMethod.Get,
                url);

        // 4. Add JWT
        request.Headers.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                token);

        // 5. Call API
        var response =
            await _httpClient.SendAsync(request);

        // 6. Read response
        var responseBody =
            await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(
                $"Employee API error: " +
                $"{(int)response.StatusCode} " +
                $"{response.StatusCode}. " +
                $"{responseBody}");
        }

        // 7. Deserialize
        var result =
            System.Text.Json.JsonSerializer
                .Deserialize<
                    ApiResponse<PagedResult<EmployeeDto>>>(
                    responseBody,
                    new System.Text.Json.JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

        return result?.Data
            ?? new PagedResult<EmployeeDto>();
    }

    public async Task<EmployeeDto?> GetEmployeeByIdAsync(Guid id)
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
                $"api/Employees/{id}");

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
                $"Get employee failed: " +
                $"{(int)response.StatusCode} " +
                $"{response.StatusCode}. " +
                $"{responseBody}");
        }

        var result =
            System.Text.Json.JsonSerializer
                .Deserialize<
                    ApiResponse<EmployeeDto>>(
                        responseBody,
                        new System.Text.Json.JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

        return result?.Data;
    }


    public async Task<bool> CreateEmployeeAsync(CreateEmployeeRequest employee)
    {
        // 1. Get JWT
        var token =
            await _tokenStorage.GetTokenAsync();

        if (string.IsNullOrWhiteSpace(token))
        {
            throw new Exception(
                "JWT token is not available.");
        }

        // 2. Create POST request
        using var request =
            new HttpRequestMessage(
                HttpMethod.Post,
                "api/Employees");

        // 3. Add JWT
        request.Headers.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                token);

        // 4. Add request body
        request.Content =
            JsonContent.Create(employee);

        // 5. Call API
        var response =
            await _httpClient.SendAsync(request);

        // 6. Read response
        var responseBody =
            await response.Content.ReadAsStringAsync();

        // 7. Handle error
        if (!response.IsSuccessStatusCode)
        {
            //throw new HttpRequestException(
            //    $"Unable to create employee. " +
            //    $"Status: {(int)response.StatusCode} " +
            //    $"{response.StatusCode}. " +
            //    $"Response: {responseBody}");
            await EnsureSuccessAsync(response);
        }

        return true;
    }
    public async Task<bool> UpdateEmployeeAsync(Guid id, UpdateEmployeeRequest employee)
    {
        // 1. Get JWT
        var token =
            await _tokenStorage.GetTokenAsync();

        if (string.IsNullOrWhiteSpace(token))
        {
            throw new Exception(
                "JWT token is not available.");
        }

        // 2. Create PUT request
        using var request =
            new HttpRequestMessage(
                HttpMethod.Put,
                $"api/Employees/{id}");

        // 3. Add JWT
        request.Headers.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                token);

        // 4. Add request body
        request.Content =
            JsonContent.Create(employee);

        // 5. Call API
        var response =
            await _httpClient.SendAsync(request);

        // 6. Read response
        var responseBody =
            await response.Content.ReadAsStringAsync();

        // 7. Handle error
        if (!response.IsSuccessStatusCode)
        {
            //throw new HttpRequestException(
            //    $"Unable to update employee. " +
            //    $"Status: {(int)response.StatusCode} " +
            //    $"{response.StatusCode}. " +
            //    $"Response: {responseBody}");
            await EnsureSuccessAsync(response);
        }

        return true;
    }
    public async Task<bool> DeleteEmployeeAsync(Guid id)
    {
        // 1. Get JWT
        var token =
            await _tokenStorage.GetTokenAsync();

        if (string.IsNullOrWhiteSpace(token))
        {
            throw new Exception(
                "JWT token is not available.");
        }

        // 2. Create DELETE request
        using var request =
            new HttpRequestMessage(
                HttpMethod.Delete,
                $"api/Employees/{id}");

        // 3. Add JWT
        request.Headers.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                token);

        // 4. Call API
        var response =
            await _httpClient.SendAsync(request);

        // 5. Read response
        var responseBody =
            await response.Content.ReadAsStringAsync();

        // 6. Handle errors
        if (!response.IsSuccessStatusCode)
        {
            //throw new HttpRequestException(
            //    $"Unable to delete employee. " +
            //    $"Status: {(int)response.StatusCode} " +
            //    $"{response.StatusCode}. " +
            //    $"Response: {responseBody}");
            await EnsureSuccessAsync(response);
        }

        return true;
    }


    private static async Task EnsureSuccessAsync(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
            return;

        var body =
            await response.Content.ReadAsStringAsync();

        string message;

        try
        {
            var error =
                System.Text.Json.JsonSerializer
                    .Deserialize<ApiErrorResponse>(
                        body,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

            message =
                error?.Message
                ?? body;
        }
        catch
        {
            message = body;
        }

        if (string.IsNullOrWhiteSpace(message))
        {
            message =
                response.StatusCode switch
                {
                    HttpStatusCode.BadRequest =>
                        "Invalid request.",

                    HttpStatusCode.Unauthorized =>
                        "Your session has expired. Please login again.",

                    HttpStatusCode.Forbidden =>
                        "You are not authorized to perform this action.",

                    HttpStatusCode.NotFound =>
                        "The requested record was not found.",

                    HttpStatusCode.InternalServerError =>
                        "An unexpected server error occurred.",

                    _ =>
                        "An error occurred while processing your request."
                };
        }

        throw new ApiException(
            (int)response.StatusCode,
            message);
    }

}