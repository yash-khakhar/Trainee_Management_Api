using TraineeManagement.api.Enum.Mentor;

namespace TraineeManagement.api.Repository
{
    public interface IMentorRepo
    {
        int Id { get; set; }
        int UserId { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string Email { get; set; }
        string Expertise { get; set; }
        MentorStatusEnum Status { get; set; }
        DateTime CreatedAt { get; set; }
        DateTime UpdatedAt { get; set; }
    }
}
