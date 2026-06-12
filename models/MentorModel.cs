using System.ComponentModel.DataAnnotations;
using TraineeManagement.api.DTO.MentorDto;
using TraineeManagement.api.DTO.TraineeDto;
using TraineeManagement.api.Enum.Mentor;
using TraineeManagement.api.models;
using TraineeManagement.api.Repository;

namespace TraineeManagement.api.Models
{
    public class MentorModel : IMentorRepo
    {
        
        public MentorModel(string firstName, string lastName, string email, string expertise, MentorStatusEnum status)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Expertise = expertise;
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

        [Required(ErrorMessage = "Expertise must be provided")]
        public string Expertise { get; set; }

        [EnumDataType(typeof(MentorStatusEnum), ErrorMessage = "Status can be either ACTIVE or INACTIVE")]
        public MentorStatusEnum Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public static MentorResponse ToDto(MentorModel mentorModel)
        {
            return new MentorResponse
                (
                    mentorModel.Id,
                    mentorModel.FirstName,
                    mentorModel.LastName,
                    mentorModel.Email,
                    mentorModel.Expertise,
                    mentorModel.Status,
                    mentorModel.CreatedAt,
                    mentorModel.UpdatedAt
                );
        }
    }
}
