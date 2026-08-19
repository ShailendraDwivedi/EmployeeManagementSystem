namespace EmployeeManagement.Blazor.Models;

public class ApiErrorResponse
{
    public bool Success { get; set; }

    public string? Message { get; set; }

    public Dictionary<string, string[]>? Errors { get; set; }
}