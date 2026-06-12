using System.Net;
using System.Text.Json;
using TraineeManagement.api.CustomException;

namespace TraineeManagement.api.Middleware
{
    public class HttpStatusCodeHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<HttpStatusCodeHandler> _logger;

        public HttpStatusCodeHandler(RequestDelegate next, ILogger<HttpStatusCodeHandler> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);

                // If headers are already sent, we cannot safely modify the response body or status code
                if (context.Response.HasStarted)
                {
                    return;
                }

                switch (context.Response.StatusCode)
                {
                    case StatusCodes.Status401Unauthorized:
                        _logger.LogCritical("User tried to log in but failed (Unauthorized).");
                        await WriteErrorResponse(context, "Unauthorized Action", StatusCodes.Status401Unauthorized, "You are unauthorized to access.");
                        break;

                    case StatusCodes.Status403Forbidden:
                        _logger.LogError("ERROR: User tried to log in but failed (Forbidden).");
                        await WriteErrorResponse(context, "Forbidden Action", StatusCodes.Status403Forbidden, "You are forbidden to access the resource.");
                        break;

                    case StatusCodes.Status404NotFound:
                        _logger.LogError("ERROR: Invalid Resource Requested");
                        await WriteErrorResponse(context, "Resource Not Found", StatusCodes.Status404NotFound, "Invalid Resource Found.");
                        break;

                    default:
                        await WriteErrorResponse(context, "Internal Server Error", StatusCodes.Status500InternalServerError, "Internal Server Error");
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception escaped the pipeline: {Message}", ex.Message);

                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
           
            if (context.Response.HasStarted)
            {
                _logger.LogWarning("The response has already started. Cannot write exception payload.");
                return;
            }

            var statusCode = StatusCodes.Status500InternalServerError;
            var errorType = "Internal Server Error";
            var message = "An unexpected error occurred on our server. Please try again later.";

            if (exception is NotFoundException)
            {
                statusCode = StatusCodes.Status404NotFound;
                errorType = "Resource Not Found";
                message = exception.Message;
            }

            
            context.Response.StatusCode = statusCode;

            await WriteErrorResponse(context, errorType, statusCode, message);
        }

        private static async Task WriteErrorResponse(HttpContext context, string errorType, int statusCode, string message)
        {
            
            context.Response.ContentType = "application/json";

            var payload = new
            {
                ErrorType = errorType,
                StatusCode = statusCode,
                Message = message
            };

            var json = JsonSerializer.Serialize(payload);
            await context.Response.WriteAsync(json);
        }
    }
}
