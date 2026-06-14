// WMS.API/Middleware/GlobalExceptionMiddleware.cs
using System.Net;
using System.Text.Json;
using WMS.Application.Common.Exceptions;
using WMS.Application.Common.Models;
using ValidationException = WMS.Application.Common.Exceptions.ValidationException;

namespace WMS.API.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next,
        ILogger<GlobalExceptionMiddleware> logger)
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
            _logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";

        var (statusCode, message, errors) = ex switch
        {
            NotFoundException => (HttpStatusCode.NotFound, ex.Message, (object?)null),
            ValidationException ve => (HttpStatusCode.BadRequest, "Validation failed.", (object?)ve.Errors),
            UnauthorizedException => (HttpStatusCode.Unauthorized, ex.Message, (object?)null),
            ConflictException => (HttpStatusCode.Conflict, ex.Message, (object?)null),
            _ => (
    HttpStatusCode.InternalServerError,
    ex.ToString(),
    (object?)null
)
        };

        context.Response.StatusCode = (int)statusCode;

        var response = ApiResponse<object>.Fail(message, (int)statusCode, errors);
        var json = JsonSerializer.Serialize(response,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        await context.Response.WriteAsync(json);
    }
}