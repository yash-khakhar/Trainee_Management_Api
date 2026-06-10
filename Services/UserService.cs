using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using TraineeManagement.api.Data;
using TraineeManagement.api.DTO.UserDto;
using TraineeManagement.api.Models;
using TraineeManagement.api.Repository;

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

        private string GenerateJwtToken(UserModel user)
        {
            var tokenHandler = new JsonWebTokenHandler();

            var jwtSettings = _config.GetSection("JwtSettings");

            if (jwtSettings == null) throw new Exception("Jwt is not configured!");

            var secretKey = jwtSettings["SecretKey"];

            var key = Encoding.UTF8.GetBytes(secretKey!);

            var expiry = jwtSettings["ExpiryMinutes"];

            var issuer = jwtSettings["Issuer"];

            var audience = jwtSettings["Audience"];

            if(issuer == null || audience == null || expiry == null || secretKey == null) throw new Exception("Jwt is not configured");

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(expiry)),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                ),
                Issuer = issuer,
                Audience = audience
            };

            return tokenHandler.CreateToken(tokenDescriptor);

        }

        public async Task<UserLoginResponse> Login(UserLoginRequestDto userDto)
        {

            UserModel user = await FindByUsername(userDto.UserName);

            if (!_passwordService.VerifyPassword(userDto.Password, user.PasswordHash))
            {
                throw new Exception("Invalid username or password");
            }

            string jwtToken = GenerateJwtToken(user);

            var jwtSettings = _config.GetSection("JwtSettings");

            var expiry = jwtSettings["ExpiryMinutes"];

            return new UserLoginResponse(jwtToken, Convert.ToInt32(expiry) * 60, new UserResponse(user.Id, user.UserName, user.Role));

        }

        public async Task<UserResponse> RegisterUser(CreateUserRequest newUser)
        {
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
