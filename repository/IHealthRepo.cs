namespace TraineeManagement.api.repository
{
    public interface IHealthRepo
    {
        string Status { get; set; }

        string ApplicationName { get; set; }

        DateTime Time { get; set; }
    }
}
