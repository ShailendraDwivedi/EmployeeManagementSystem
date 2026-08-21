namespace EmployeeManagement.Application.Common.Models;

public class ApiResponse
{
    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;

    public Dictionary<string, string[]>? Errors { get; set; }

    public static ApiResponse Ok(string message = "Request successful.")
    {
        return new ApiResponse
        {
            Success = true,
            Message = message
        };
    }

    public static ApiResponse Fail(string message)
    {
        return new ApiResponse
        {
            Success = false,
            Message = message
        };
    }
}

public class ApiResponse<T> : ApiResponse
{
    public T? Data { get; set; }

    public static ApiResponse<T> SuccessResponse(
        T data,
        string message = "Request successful.")
    {
        return new ApiResponse<T>
        {
            Success = true,
            Message = message,
            Data = data
        };
    }
}