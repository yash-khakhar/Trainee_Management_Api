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

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserResponse>> RegisterUser([FromBody] CreateUserRequest user)
        {
            try
            {

                if (user == null)
                {
                    return BadRequest("Please provide correct input");
                }

                return await _userService.RegisterUser(user);
            }

            catch (Exception ex)
            {

                var errorResponse = new { message = ex.Message };

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
                    return BadRequest("Please provide correct input");
                }

                var userLoginResponse = await _userService.Login(user);

                return Ok(userLoginResponse);
            }

            catch (Exception ex)
            {

                var errorResponse = new { message = ex.Message };

                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }


        }

    }
}
