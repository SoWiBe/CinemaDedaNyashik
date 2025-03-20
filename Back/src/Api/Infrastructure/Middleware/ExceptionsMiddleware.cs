using System.Net;
using Back.Api.Infrastructure.Exceptions;

namespace Back.Api.Infrastructure.Middleware;

public class ExceptionsMiddleware
{
    private const string ContentType =  "application/json";
    
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionsMiddleware> _logger;

    public ExceptionsMiddleware(RequestDelegate next, ILogger<ExceptionsMiddleware> logger)
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
        catch (CustomException ex)
        {
            await HandleExceptionAsync(context, ex);
        }
        catch (Exception ex)
        {
            await SetupContext(context, new ExceptionResponse(HttpStatusCode.InternalServerError, ex.Message));
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, CustomException exception)
    {
        _logger.LogError(exception, exception.Message);

        var response = exception.StatusCode switch
        {
            404 => new ExceptionResponse(HttpStatusCode.NotFound, exception.Message),
            403 => new ExceptionResponse(HttpStatusCode.Forbidden, exception.Message),
            401 => new ExceptionResponse(HttpStatusCode.Unauthorized, exception.Message),
            400 => new ExceptionResponse(HttpStatusCode.BadRequest, exception.Message),
            _ => new ExceptionResponse(HttpStatusCode.InternalServerError, exception.Message),
        };

        await SetupContext(context, response);
    }
    
    private static async Task SetupContext(HttpContext context, ExceptionResponse response)
    {
        context.Response.ContentType = ContentType;
        context.Response.StatusCode = (int)response.StatusCode;
        await context.Response.WriteAsJsonAsync(response);
    }
}

public record ExceptionResponse(HttpStatusCode StatusCode, string Description);