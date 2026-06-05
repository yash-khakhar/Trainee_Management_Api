using System.ComponentModel.DataAnnotations;
using TraineeManagement.api.Enum.Trainee;

namespace TraineeManagement.api.DTO.TraineeDto
{
    public class CreateTraineeRequest
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
        
        [Required(ErrorMessage = "TechStack must be provided")]
        public required string TechStack { get; set; }
        
        [EnumDataType(typeof(TraineeStatusEnum), ErrorMessage = "Status can be either ACTIVE or INACTIVE")]
        public required TraineeStatusEnum Status { get; set; }

    }
}
