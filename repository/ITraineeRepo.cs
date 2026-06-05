using TraineeManagement.api.Enum.Trainee;

namespace TraineeManagement.api.repository
{
    public interface ITraineeRepo
    {
        string Id { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string Email { get; set; }
        string TechStack { get; set; }
        TraineeStatusEnum Status { get; set; }
        DateTime CreatedAt { get; set; }
        DateTime UpdatedAt { get; set; }
    }
}
