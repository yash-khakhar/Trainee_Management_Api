using System.Diagnostics;

namespace TraineeManagement.api.Middleware
{
    public class CorrelationIdHandler : DelegatingHandler
    {
        private const string CorrelationIdHeader = "X-Correlation-ID";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CorrelationIdHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            string? correlationId = httpContext?.Request.Headers[CorrelationIdHeader].FirstOrDefault();

            if (string.IsNullOrEmpty(correlationId))
            {
                correlationId = Activity.Current?.RootId ?? Guid.NewGuid().ToString();
            }

            if (!request.Headers.Contains(CorrelationIdHeader))
            {
                request.Headers.Add(CorrelationIdHeader, correlationId);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
