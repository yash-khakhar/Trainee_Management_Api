using TraineeManagement.api.Enum.Mentor;

namespace TraineeManagement.api.DTO.MentorDto
{
    public class MentorResponse
    {
        public MentorResponse(int id, string firstName, string lastName, string email, string expertise, MentorStatusEnum status, DateTime createdAt, DateTime updatedAt)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Expertise = expertise;
            Status = status;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        public MentorResponse() { }

        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = String.Empty;
        public string Email { get; set; } = string.Empty;
        public string Expertise { get; set; } = string.Empty;
        public MentorStatusEnum Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
