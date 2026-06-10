using TraineeManagement.api.Enum.User;

namespace TraineeManagement.api.DTO.UserDto
{
    public class UserResponse
    {
        public UserResponse(int id, string userName, UserRolesEnum role)
        {
            Id = id;
            UserName = userName;
            Role = role;
        }
        
        public int Id { get; set; }
        public string UserName { get; set; }
        public UserRolesEnum Role { get; set; }
    }
}
