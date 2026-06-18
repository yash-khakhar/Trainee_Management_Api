using System.ComponentModel.DataAnnotations;
using TraineeManagement.api.Enum;

namespace TraineeManagement.api.DTO.TraineeDto
{
    public class CreateTraineeRequest
    {
        public CreateTraineeRequest(string firstName, string lastName, string email, string techStack, TraineeStatusEnum? status)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            TechStack = techStack;
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
        
        [Required(ErrorMessage = "TechStack must be provided")]
        public string TechStack { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [EnumDataType(typeof(TraineeStatusEnum), ErrorMessage = "Status can be either ACTIVE or INACTIVE")]
        public TraineeStatusEnum? Status { get; set; }

    }
}
