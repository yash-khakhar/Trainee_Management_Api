using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TraineeManagement.api.DTO.UserDto;
using TraineeManagement.api.Enum;
using TraineeManagement.api.Repository.User;

namespace TraineeManagement.api.Models
{
    [Index(nameof(UserName), IsUnique = true)]
    public class UserModel : IUserRepo
    {

        public UserModel(string userName, string email, string passwordHash, UserRolesEnum role) { 
            UserName = userName;
            Email = email;
            PasswordHash = passwordHash;
            Role = role;
        }

        [Key]
        public int Id { get;  set; }
        
        [Required]
        [StringLength(50, ErrorMessage = "UserName cannot be more than 50 characters")]
        public string UserName { get; set; }
        
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }
        
        [Required]
        public string PasswordHash { get; set; }

        [EnumDataType(typeof(UserRolesEnum), ErrorMessage = "Role can be either ADMIN, MENTOR or TRAINEE")]
        public UserRolesEnum Role { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public static UserResponse ToDto(UserModel userModel)
        {
            return new UserResponse
                (
                    userModel.Id,
                    userModel.UserName,
                    userModel.Role
                );
        }
    }
}
