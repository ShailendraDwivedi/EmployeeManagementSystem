using EmployeeManagement.Blazor.Models;
using EmployeeManagement.Blazor.Models.Attendance;
using System;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace EmployeeManagement.Blazor.Services.Attendance;

public class AttendanceService : IAttendanceService
{
    private readonly HttpClient _httpClient;
    private readonly ITokenStorageService _tokenStorage;

    public AttendanceService(IHttpClientFactory httpClientFactory, ITokenStorageService tokenStorage)
    {
        _httpClient = httpClientFactory.CreateClient("EmployeeAPI");
        _tokenStorage = tokenStorage;
    }

    public async Task<PagedResult<AttendanceDto>> GetAttendancesAsync(int pageNumber = 1, int pageSize = 10, string? search = null, Guid? employeeId = null, DateTime? fromDate = null, DateTime? toDate = null, string? status = null)
    {
        var token = await _tokenStorage.GetTokenAsync();

        if (string.IsNullOrWhiteSpace(token))
        {
            throw new Exception("JWT token is not available.");
        }
        var parameters = new List<string> { $"pageNumber={pageNumber}", $"pageSize={pageSize}" };

        if (!string.IsNullOrWhiteSpace(search))
        {
            parameters.Add($"search={Uri.EscapeDataString(search)}");
        }

        if (employeeId.HasValue)
        {
            parameters.Add($"employeeId={employeeId.Value}");
        }

        if (fromDate.HasValue)
        {
            parameters.Add($"fromDate={fromDate.Value:yyyy-MM-dd}");
        }

        if (toDate.HasValue)
        {
            parameters.Add($"toDate={toDate.Value:yyyy-MM-dd}");
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            parameters.Add($"status={Uri.EscapeDataString(status)}");
        }

        var url = "api/attendance?" + string.Join("&", parameters);


        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.SendAsync(request);


        var responseBody = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            var error =
                await response.Content.ReadAsStringAsync();

            throw new Exception(
                $"Attendance API failed. Status: {(int)response.StatusCode} " +
                $"{response.StatusCode}. Response: {error}");
        }

        var result =
            await response.Content
                .ReadFromJsonAsync<PagedResult<AttendanceDto>>();

        return result ??
               new PagedResult<AttendanceDto>();
    }

    public async Task<AttendanceDto?> GetAttendanceByIdAsync(Guid id)
    {
        var token = await _tokenStorage.GetTokenAsync();

        if (string.IsNullOrWhiteSpace(token))
        {
            throw new Exception("JWT token is not available.");
        }

        using var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"api/attendance/{id}");

        request.Headers.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                token);

        var response = await _httpClient.SendAsync(request);

        if (response.StatusCode ==
            System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<AttendanceDto>();
    }

    public async Task<Guid?> CheckInAsync(Guid employeeId)
    {
        var token = await _tokenStorage.GetTokenAsync();

        if (string.IsNullOrWhiteSpace(token))
        {
            throw new Exception("JWT token is not available.");
        }

        using var request = new HttpRequestMessage(HttpMethod.Post, "api/attendance/check-in");

        // Add JWT token
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Send EmployeeId in request body
        request.Content = JsonContent.Create(new
        {
            employeeId = employeeId,
        });

        // Send request
        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var result = await response.Content.ReadFromJsonAsync<CheckInResponse>();

        return result?.AttendanceId;
    }

    public async Task<bool> CheckOutAsync(Guid attendanceId)
    {
        var token = await _tokenStorage.GetTokenAsync();

        if (string.IsNullOrWhiteSpace(token))
        {
            throw new Exception("JWT token is not available.");
        }

        using var request = new HttpRequestMessage(HttpMethod.Put, $"api/attendance/check-out/{attendanceId}");

        // Add JWT token
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Send request
        var response = await _httpClient.SendAsync(request);

        return response.IsSuccessStatusCode;
    }
    public async Task<PagedResult<AttendanceDto>> GetEmployeeAttendancesAsync(Guid employeeId, int pageNumber = 1, int pageSize = 10)
    {
        var token = await _tokenStorage.GetTokenAsync();

        if (string.IsNullOrWhiteSpace(token))
        {
            throw new Exception("JWT token is not available.");
        }

        var url = $"api/attendance/employee/{employeeId}" + $"?pageNumber={pageNumber}" + $"&pageSize={pageSize}";

        using var request = new HttpRequestMessage(HttpMethod.Get, url);

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<PagedResult<AttendanceDto>>();

        return result ?? new PagedResult<AttendanceDto>();
    }
}