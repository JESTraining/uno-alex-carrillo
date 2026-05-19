using IssueTracker.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace IssueTracker.Api.Middleware;

public class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

    public GlobalExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionHandlingMiddleware> logger)
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
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = new ErrorResponse();

        switch (exception)
        {
            case NotFoundException notFound:
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                response.Message = notFound.Message;
                response.ErrorCode = "NOT_FOUND";
                break;

            case BadRequestException badRequest:
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                response.Message = badRequest.Message;
                response.ErrorCode = "BAD_REQUEST";
                break;

            case UnauthorizedException unauthorized:
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                response.Message = unauthorized.Message;
                response.ErrorCode = "UNAUTHORIZED";
                break;

            case ValidationException validation:
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                response.Message = "Validation failed";
                response.ErrorCode = "VALIDATION_ERROR";
                response.Details = validation.Errors?
                    .GroupBy(x => x.PropertyName)
                    .ToDictionary(
                        x => x.Key,
                        x => x.Select(e => e.ErrorMessage).ToList());
                break;

            default:
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                response.Message = "An unexpected error occurred";
                response.ErrorCode = "INTERNAL_SERVER_ERROR";
                break;
        }

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var jsonResponse = JsonSerializer.Serialize(response, options);
        return context.Response.WriteAsync(jsonResponse);
    }
}

public class ErrorResponse
{
    public string Message { get; set; } = string.Empty;

    public string ErrorCode { get; set; } = string.Empty;

    public Dictionary<string, List<string>>? Details { get; set; }
}
