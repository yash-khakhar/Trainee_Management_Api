using TraineeManagement.api.Repository.Health;

namespace TraineeManagement.api.models
{
    public class HealthModel : IHealthRepo
    {
        public string Status { get; set; } = string.Empty;

        public string ApplicationName { get; set; } = string.Empty;

        public DateTime Time { get; set; }
    }
}
