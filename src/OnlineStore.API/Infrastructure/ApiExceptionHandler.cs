using Microsoft.AspNetCore.Diagnostics;
using OnlineStore.API.Contracts;
using OnlineStore.BLL.Exceptions;

namespace OnlineStore.API.Infrastructure;

public class ApiExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var statusCode = exception switch
        {
            NotFoundException => StatusCodes.Status404NotFound,
            ConflictException => StatusCodes.Status409Conflict,
            BusinessRuleException => StatusCodes.Status422UnprocessableEntity,
            _ => 0
        };
        if (statusCode == 0)
        {
            return false;
        }

        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(new MessageResponse(exception.Message), cancellationToken);
        return true;
    }
}
