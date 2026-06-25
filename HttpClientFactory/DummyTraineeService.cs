using System.Net;
using TraineeManagement.api.CustomException;

namespace TraineeManagement.api.HttpClientFactory
{
    public class DummyTraineeService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<DummyTraineeService> _logger;

        public DummyTraineeService(HttpClient httpClient, ILogger<DummyTraineeService> logger)
        {
            _httpClient = httpClient;
            _logger= logger;
        }

        public async Task<DummyTraineeDto?> GetTraineeById(int id, CancellationToken cancellationToken)
        {
            try
            {
                // Appends "api/Trainee/1" to the BaseAddress
                var response = await _httpClient.GetAsync($"api/Trainee/{id}", cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    await HandleHttpRequestFailureAsync(response, cancellationToken);
                }

                var trainee = await response.Content.ReadFromJsonAsync<DummyTraineeDto>(cancellationToken);
                return trainee ?? throw new InvalidOperationException("The target API returned an empty body.");

            }
            catch (Exception ex)
            {
                // Log exception details
                _logger.LogWarning(ex, "Downstream service unavailable or circuit broken for Trainee ID {Id}.", id);
                return new DummyTraineeDto(
                    id,
                    $"Name - {id}",
                    "Trainee",
                    "trainee@dummy.com",
                    "MERN",
                    DateTime.UtcNow,
                    DateTime.UtcNow,
                    true
                );
                
            }
        }

        private async Task HandleHttpRequestFailureAsync(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            ApiErrorResponse? errorDetails = null;
            try
            {
                errorDetails = await response.Content.ReadFromJsonAsync<ApiErrorResponse>(cancellationToken);
            }
            catch(Exception ex)
            {
                _logger.LogWarning(ex, ex.Message);
            }

            var message = errorDetails?.Detail ?? $"API error details missing.";

            // Do not retry these status codes - they map directly to specialized business exceptions
            throw response.StatusCode switch
            {
                HttpStatusCode.BadRequest => new ArgumentException($"Validation Error (400): {message}"),
                HttpStatusCode.Unauthorized => new UnauthorizedAccessException("Authentication failed (401). Access Token is invalid."),
                HttpStatusCode.Forbidden => new UnauthorizedAccessException("Authorization failed (403). Insufficient scopes."),
                HttpStatusCode.NotFound => new NotFoundException($"Resource Not Found (404)."),
                _ => new HttpRequestException($"Transient API failure [{response.StatusCode}]: {message}", null, response.StatusCode)
            };
        }


    }

    public record DummyTraineeDto(
        int Id, 
        string Name, 
        string Role, 
        string Email, 
        string TechStack, 
        DateTime CreatedAt, 
        DateTime UpdatedAt,
        bool IsFallback = false
    );

    public record ApiErrorResponse(string Title, int Status, string Detail);

}


