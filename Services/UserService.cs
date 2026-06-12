using Microsoft.EntityFrameworkCore;
using TraineeManagement.api.CustomException;
using TraineeManagement.api.Data;
using TraineeManagement.api.DTO.UserDto;
using TraineeManagement.api.Helper;
using TraineeManagement.api.Models;
using TraineeManagement.api.Repository.Password;
using TraineeManagement.api.Repository.User;

namespace TraineeManagement.api.Services
{
    public class UserService : IUserService
    {

        private AppDbContext _context;
        private IPasswordService _passwordService;
        private readonly IConfiguration _config;

        public UserService(AppDbContext context, IPasswordService passwordService, IConfiguration config)
        {
            _context = context;
            _passwordService = passwordService;
            _config = config;
        }

        private async Task<UserModel> FindByUsername(string userName)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName.Equals(userName));
            if (user == null) throw new Exception("Invalid username or password");
            return user;
        }

        private async Task<bool> isUserPresent(string username)
        {
            return _context.Users.Any(u => u.UserName.Equals(username));
            
        }

        public async Task<UserLoginResponse> Login(UserLoginRequestDto userDto)
        {

            UserModel user = await FindByUsername(userDto.UserName);

            if (!await _passwordService.VerifyPassword(userDto.Password, user))
            {
                throw new Exception("Invalid username or password");
            }

            string jwtToken = JwtHelper.GenerateJwtToken(user, _config);

            var jwtSettings = _config.GetSection("JwtSettings");

            var expiry = jwtSettings["ExpiryMinutes"];

            return new UserLoginResponse(jwtToken, Convert.ToInt32(expiry) * 60, new UserResponse(user.Id, user.UserName, user.Role));

        }

        public async Task<UserResponse> RegisterUser(CreateUserRequest newUser)
        {

            if (await isUserPresent(newUser.Username))
            {
                throw new DuplicateUsernameException($"The username '{newUser.Username}' is already taken.");
            }

            UserModel userModel = new UserModel(
                     newUser.Username,
                     newUser.Email,
                     _passwordService.HashPassword(newUser.Password),
                     newUser.Role
                     
                 );

            userModel.CreatedAt = DateTime.UtcNow;
            userModel.UpdatedAt = DateTime.UtcNow;

            _context.Users.Add(userModel);
            await _context.SaveChangesAsync();

            return UserModel.ToDto(userModel);
        }
    }
}
