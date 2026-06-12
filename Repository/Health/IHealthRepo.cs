namespace TraineeManagement.api.Repository.Health
{
    public interface IHealthRepo
    {
        string Status { get; set; }

        string ApplicationName { get; set; }

        DateTime Time { get; set; }
    }
}
