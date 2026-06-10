using Microsoft.AspNetCore.Mvc;
using TraineeManagement.api.DTO.UserDto;
using TraineeManagement.api.Repository;

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
            try
            {

                if (user == null)
                {
                    _logger.LogInformation($"Exception in User Creation: Invalid input provided by user");
                    return BadRequest("Please provide correct input");
                }

                UserResponse userResponse =  await _userService.RegisterUser(user);
                
                _logger.LogInformation($"Register: {userResponse.UserName} is registered!!");
                
                return userResponse;
            }

            catch (Exception ex)
            {

                var errorResponse = new { message = ex.Message };

                _logger.LogInformation($"Exception in User Creation: {ex.Message}");

                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }


        }

        [HttpPost("login")]
        public async Task<ActionResult<UserLoginResponse>> Login([FromBody] UserLoginRequestDto user)
        {
            try
            {

                if (user == null)
                {
                    _logger.LogInformation($"Exception in User Login: Invalid Credentials");
                    return BadRequest("Please provide correct input");
                }

                var userLoginResponse = await _userService.Login(user);

                _logger.LogInformation($"Login: {userLoginResponse.User.UserName} is logged in!!");

                return userLoginResponse;
            }

            catch (Exception ex)
            {

                var errorResponse = new { message = ex.Message };

                _logger.LogInformation($"Exception in User Login: {ex.Message}");

                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }


        }

    }
}
