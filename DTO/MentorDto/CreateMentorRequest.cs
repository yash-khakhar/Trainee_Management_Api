using System.ComponentModel.DataAnnotations;
using TraineeManagement.api.Enum.Mentor;

namespace TraineeManagement.api.DTO.MentorDto
{
    public class CreateMentorRequest
    {
        [Required]
        [StringLength(50, ErrorMessage = "First Name cannot be more than 50 characters")]
        public required string FirstName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Last Name cannot be more than 50 characters")]
        public required string LastName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Expertise must be provided")]
        public required string Expertise { get; set; }

        [EnumDataType(typeof(MentorStatusEnum), ErrorMessage = "Status can be either ACTIVE or INACTIVE")]
        public required MentorStatusEnum Status { get; set; }
    }
}
