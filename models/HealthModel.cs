using TraineeManagement.api.Repository.Health;

namespace TraineeManagement.api.models
{
    public class HealthModel : IHealthRepo
    {
        public string Status { get; set; } 

        public string ApplicationName { get; set; }

        public DateTime Time { get; set; }
    }
}
