using System.ComponentModel.DataAnnotations;
using TraineeManagement.api.Enum.Trainee;

namespace TraineeManagement.api.DTO.TraineeDto
{
    public class TraineeResponse
    {
        public TraineeResponse(string id, string firstName, string lastName, string email, string techStack, TraineeStatusEnum status, DateTime createdAt, DateTime updatedAt)
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

        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string TechStack { get; set; }
        public TraineeStatusEnum Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
