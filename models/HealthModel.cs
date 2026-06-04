using TraineeManagement.api.repository;

namespace TraineeManagement.api.models
{
    public class HealthModel : IHealthRepo
    {
        public string Status { get; set; } 

        public string ApplicationName { get; set; }

        public DateTime Time { get; set; }
    }
}
