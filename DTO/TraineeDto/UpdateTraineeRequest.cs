using System.ComponentModel.DataAnnotations;
using TraineeManagement.api.Enum.Trainee;

namespace TraineeManagement.api.DTO.TraineeDto
{
    public class UpdateTraineeRequest
    {
        [Required]
        public int Id { get; set; }

        [StringLength(50, ErrorMessage = "First Name cannot be more than 50 characters")]
        public string? FirstName { get; set; }

        [StringLength(50, ErrorMessage = "Last Name cannot be more than 50 characters")]
        public string? LastName { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string? Email { get; set; }

        public string? TechStack { get; set; }

        [EnumDataType(typeof(TraineeStatusEnum), ErrorMessage = "Status can be either ACTIVE or INACTIVE")]
        public TraineeStatusEnum Status { get; set; }
    }
}
