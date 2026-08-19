using FluentValidation;
using System.Net;

namespace EmployeeManagement.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    private readonly ILogger<ExceptionHandlingMiddleware>
        _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            await HandleValidationExceptionAsync(
                context,
                ex);
        }
        catch (KeyNotFoundException ex)
        {
            await HandleKeyNotFoundExceptionAsync(
                context,
                ex);
        }
        catch (UnauthorizedAccessException ex)
        {
            await HandleUnauthorizedExceptionAsync(
                context,
                ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "An unhandled exception occurred.");

            await HandleExceptionAsync(
                context,
                ex);
        }
    }

    // ==========================================
    // Validation Exception
    // ==========================================

    private static async Task
        HandleValidationExceptionAsync(
            HttpContext context,
            ValidationException exception)
    {
        context.Response.StatusCode =
            (int)HttpStatusCode.BadRequest;

        context.Response.ContentType =
            "application/json";

        var errors =
            exception.Errors
                .GroupBy(x => x.PropertyName)
                .ToDictionary(
                    group => group.Key,
                    group => group
                        .Select(x => x.ErrorMessage)
                        .ToArray());

        await context.Response.WriteAsJsonAsync(
            new
            {
                success = false,

                message = "Validation failed.",

                errors
            });
    }

    // ==========================================
    // Not Found
    // ==========================================

    private static async Task
        HandleKeyNotFoundExceptionAsync(
            HttpContext context,
            KeyNotFoundException exception)
    {
        context.Response.StatusCode =
            (int)HttpStatusCode.NotFound;

        context.Response.ContentType =
            "application/json";

        await context.Response.WriteAsJsonAsync(
            new
            {
                success = false,

                message = exception.Message
            });
    }

    // ==========================================
    // Unauthorized
    // ==========================================

    private static async Task
        HandleUnauthorizedExceptionAsync(
            HttpContext context,
            UnauthorizedAccessException exception)
    {
        context.Response.StatusCode =
            (int)HttpStatusCode.Unauthorized;

        context.Response.ContentType =
            "application/json";

        await context.Response.WriteAsJsonAsync(
            new
            {
                success = false,

                message = exception.Message
            });
    }

    // ==========================================
    // General Exception
    // ==========================================

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        context.Response.ContentType = "application/json";

        await context.Response.WriteAsJsonAsync(
            new
            {
                success = false,
                message = exception.Message
                // "An unexpected error occurred."
            });
    }
}