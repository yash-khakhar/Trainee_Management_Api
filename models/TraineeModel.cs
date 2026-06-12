using System.ComponentModel.DataAnnotations;
using TraineeManagement.api.DTO.TraineeDto;
using TraineeManagement.api.Enum;
using TraineeManagement.api.Repository.Trainee;

namespace TraineeManagement.api.models
{
    public class TraineeModel: ITraineeRepo
    {
        public TraineeModel(string firstName, string lastName, string email, string techStack, TraineeStatusEnum status)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            TechStack = techStack;
            Status = status;
        }

        [Key]
        public int Id { get; set; }
        
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

        [EnumDataType(typeof(TraineeStatusEnum), ErrorMessage = "Status can be either ACTIVE or INACTIVE")]
        public TraineeStatusEnum Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public static TraineeResponse ToDto(TraineeModel traineeModel) 
        {
            return new TraineeResponse
                (
                    traineeModel.Id,
                    traineeModel.FirstName,
                    traineeModel.LastName,
                    traineeModel.Email,
                    traineeModel.TechStack,
                    traineeModel.Status,
                    traineeModel.CreatedAt,
                    traineeModel.UpdatedAt
                );
        }

    }
}
