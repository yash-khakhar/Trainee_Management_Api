namespace TraineeManagement.api.Middleware
{
    public class GlobalException
    {
        private readonly RequestDelegate _next;

        public GlobalException(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // for running next middleware down the pipeline
            await _next(context);

            // Intercept if something else downstream set the status to 403
            if (context.Response.StatusCode == StatusCodes.Status403Forbidden && !context.Response.HasStarted)
            {
                context.Response.ContentType = "application/json";

                var payload = new
                {
                    title = "Forbidden Action",
                    status = 403,
                    detail = "You are forbidden to access the resource."
                };

                await context.Response.WriteAsJsonAsync(payload);
            }

            if (context.Response.StatusCode == StatusCodes.Status401Unauthorized && !context.Response.HasStarted)
            {
                context.Response.ContentType = "application/json";

                var payload = new
                {
                    title = "Unauthorized Action",
                    status = 401,
                    detail = "You are unauthorized to access."
                };

                await context.Response.WriteAsJsonAsync(payload);
            }
        }
    }
}
