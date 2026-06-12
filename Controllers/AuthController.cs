using Microsoft.AspNetCore.Mvc;
using TraineeManagement.api.DTO.UserDto;
using TraineeManagement.api.Repository.User;

namespace TraineeManagement.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IUserService userService, ILogger<AuthController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserResponse>> RegisterUser([FromBody] CreateUserRequest user)
        {
            if (user == null)
            {
                throw new Exception("Please Provide Correct Input Data");
            }

            UserResponse userResponse = await _userService.RegisterUser(user);

            _logger.LogInformation($"Register: {userResponse.UserName} is registered!!");

            return userResponse;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserLoginResponse>> Login([FromBody] UserLoginRequestDto user)
        {
            if (user == null)
            {
                _logger.LogInformation($"ERROR: Exception in User Login: Invalid Credentials");
                throw new Exception("Invalid Credentails");
            }

            var userLoginResponse = await _userService.Login(user);

            _logger.LogInformation($"Login: {userLoginResponse.User.UserName} is logged in!!");

            return userLoginResponse;

        }

    }
}
