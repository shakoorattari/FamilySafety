using System.Net;
using System.Text.Json;
using FluentValidation;

namespace FamilySafety.API.Middleware;

/// <summary>
/// Global exception handling middleware
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "An unhandled exception occurred");

        var response = context.Response;
        response.ContentType = "application/json";

        var (statusCode, message) = exception switch
        {
            ValidationException validationEx => (HttpStatusCode.BadRequest, GetValidationErrors(validationEx)),
            KeyNotFoundException => (HttpStatusCode.NotFound, "Resource not found"),
            UnauthorizedAccessException => (HttpStatusCode.Unauthorized, "Unauthorized access"),
            ArgumentException argEx => (HttpStatusCode.BadRequest, argEx.Message),
            InvalidOperationException invEx => (HttpStatusCode.BadRequest, invEx.Message),
            _ => (HttpStatusCode.InternalServerError, "An internal server error occurred")
        };

        response.StatusCode = (int)statusCode;

        var errorResponse = new ErrorResponse
        {
            StatusCode = (int)statusCode,
            Message = message,
            Timestamp = DateTime.UtcNow
        };

        var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        await response.WriteAsync(JsonSerializer.Serialize(errorResponse, jsonOptions));
    }

    private static string GetValidationErrors(ValidationException exception)
    {
        var errors = exception.Errors.Select(e => e.ErrorMessage);
        return string.Join("; ", errors);
    }
}

/// <summary>
/// Standard error response model
/// </summary>
public class ErrorResponse
{
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}
