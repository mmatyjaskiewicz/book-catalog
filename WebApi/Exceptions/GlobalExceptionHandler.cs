using Application.Exceptions;
using Application.Exceptions.BadRequest;
using Application.Exceptions.Conflict;
using Application.Exceptions.NotFound;
using Microsoft.AspNetCore.Diagnostics;

namespace WebApi.Exceptions;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(exception, "An unhandled exception occurred while processing the request.");
        
        var title = exception switch
        {
            BadRequestException => "Bad Request",
            NotFoundException => "Not Found",
            ConflictException => "Conflict",
            _ => "Internal Server Error"
        };
        
        var statusCode = exception switch
        {
            BadRequestException => StatusCodes.Status400BadRequest,
            NotFoundException => StatusCodes.Status404NotFound,
            ConflictException => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };
        
        var message = exception switch
        {
            AppException => exception.Message,
            _ => "Internal server error"
        };

        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsJsonAsync(new
        {
            Title = title,
            Status = statusCode,
            error = message
        }, cancellationToken);

        return true;
    }
}