using System.ComponentModel.DataAnnotations;
using TraineeManagement.api.Enum;

namespace TraineeManagement.api.DTO.UserDto
{
    public class CreateUserRequest
    {
        [Required]
        [StringLength(50, ErrorMessage = "First Name cannot be more than 50 characters")]
        public required string Username { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "TechStack must be provided")]
        public required string Password { get; set; }

        [EnumDataType(typeof(UserRolesEnum), ErrorMessage = "Role can be either ADMIN, MENTOR or TRAINEE")]
        public required UserRolesEnum Role { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "First Name cannot be more than 50 characters")]
        public required string FirstName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Last Name cannot be more than 50 characters")]
        public required string LastName { get; set; }

        [Required(ErrorMessage = "TechStack must be provided")]
        public required string TechStack { get; set; }

        [EnumDataType(typeof(UserStatusEnum), ErrorMessage = "Status can be either ACTIVE or INACTIVE")]
        public required UserStatusEnum Status { get; set; }
    }
}
