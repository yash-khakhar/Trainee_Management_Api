using TraineeManagement.api.Enum;

namespace TraineeManagement.api.DTO.TraineeDto
{
    public class TraineeResponse
    {
        public TraineeResponse(int id, string firstName, string lastName, string email, string techStack, TraineeStatusEnum? status, DateTime createdAt, DateTime updatedAt)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            TechStack = techStack;
            Status = status;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        public TraineeResponse() { }

        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string TechStack { get; set; } = string.Empty;
        public TraineeStatusEnum? Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
