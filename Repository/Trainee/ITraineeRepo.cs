using TraineeManagement.api.Enum;

namespace TraineeManagement.api.Repository.Trainee
{
    public interface ITraineeRepo
    {
        int Id { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string Email { get; set; }
        string TechStack { get; set; }
        TraineeStatusEnum Status { get; set; }
        DateTime CreatedAt { get; set; }
        DateTime UpdatedAt { get; set; }
    }
}
