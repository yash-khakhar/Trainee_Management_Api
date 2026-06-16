using System.ComponentModel.DataAnnotations;
using TraineeManagement.api.Enum.Mentor;

namespace TraineeManagement.api.DTO.MentorDto
{
    public class CreateMentorRequest
    {
        
        public CreateMentorRequest(string firstName, string lastName, string email, string expertise, MentorStatusEnum status)
        { 
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Expertise = expertise;
            Status = status;
        }
        
        [Required]
        [StringLength(50, ErrorMessage = "First Name cannot be more than 50 characters")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Last Name cannot be more than 50 characters")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Expertise must be provided")]
        public string Expertise { get; set; }

        [EnumDataType(typeof(MentorStatusEnum), ErrorMessage = "Status can be either ACTIVE or INACTIVE")]
        public MentorStatusEnum Status { get; set; }
    }
}
