using System.ComponentModel.DataAnnotations;
using TraineeManagement.api.Enum.Mentor;

namespace TraineeManagement.api.DTO.MentorDto
{
    public class UpdateMentorRequest
    {

        [StringLength(50, ErrorMessage = "First Name cannot be more than 50 characters")]
        public string? FirstName { get; set; }

        [StringLength(50, ErrorMessage = "Last Name cannot be more than 50 characters")]
        public string? LastName { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string? Email { get; set; }

        public string? Expertise { get; set; }

        [EnumDataType(typeof(MentorStatusEnum), ErrorMessage = "Status can be either ACTIVE or INACTIVE")]
        public MentorStatusEnum Status { get; set; }
    }
}
