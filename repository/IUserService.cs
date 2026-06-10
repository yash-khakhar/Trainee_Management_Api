using TraineeManagement.api.DTO.UserDto;

namespace TraineeManagement.api.Repository
{
    public interface IUserService
    {
        public Task<UserResponse> RegisterUser(CreateUserRequest newUser);
        public Task<UserLoginResponse> Login(UserLoginRequestDto userDto);
    }
}
