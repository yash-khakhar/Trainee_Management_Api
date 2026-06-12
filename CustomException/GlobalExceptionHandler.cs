using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace TraineeManagement.api.CustomException
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken
        )
        {
            _logger.LogError(exception, "An exception occurred: {Message}", exception.Message);

            var (statusCode, title) = exception switch
            {
                NotFoundException => (StatusCodes.Status400BadRequest, "Resource Not Found"),
                UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "Unauthorised Excess"),
                _ => (StatusCodes.Status500InternalServerError, "Internal Server Error")
            };

            var problemDetails = new ProblemDetails
            {
                Title = title,
                Detail = exception.Message,
                Instance = httpContext.Request.Path
            };

            httpContext.Response.StatusCode = statusCode;

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;

        }
    }
}
