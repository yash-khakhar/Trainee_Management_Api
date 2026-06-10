using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraineeManagement.api.DTO.TraineeDto;
using TraineeManagement.api.Enum.User;
using TraineeManagement.api.repository;

namespace TraineeManagement.api.Controllers
{
 
    [Route("api/[controller]")]
    [Authorize(Roles = nameof(UserRolesEnum.ADMIN))]
    [ApiController]
    public class TraineeController : ControllerBase
    {

        private readonly ITraineeService _traineeServices;
        public TraineeController(ITraineeService traineeServices)
        {
            _traineeServices = traineeServices;
        }

        [HttpGet]
        [ProducesResponseType(typeof(TraineeResponse[]), StatusCodes.Status200OK)]
        public async Task<ActionResult<TraineeResponse[]>> ListAllTrainee([FromQuery(Name="searchKeyword")] string? searchKeyword = null )
        {
            try
            {
                if(searchKeyword != null)
                {
                    return Ok(await _traineeServices.SearchTrainee(searchKeyword.ToLower()));
                }
                else
                {
                    return Ok(await _traineeServices.GetTraineeList());
                }
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("{id}", Name = "GetTraineeById")]
        public async Task<ActionResult<TraineeResponse>> GetTraineeById(int id)
        {
            try
            {
                return Ok(await _traineeServices.GetTraineeById(id));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(TraineeResponse), StatusCodes.Status201Created)]
        public async Task<ActionResult<TraineeResponse>> AddTrainee([FromBody] CreateTraineeRequest trainee)
        {

            if (trainee == null)
            {
                return BadRequest();
            }

            try
            {
                return await _traineeServices.AddTrainee(trainee);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut]
        [ProducesResponseType(typeof(TraineeResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<TraineeResponse>> UpdateTrainee([FromBody] UpdateTraineeRequest updateTraineeRequest)
        {

            try
            {
                return Ok(await _traineeServices.UpdateTrainee(updateTraineeRequest));
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTraineeById(int id)
        {
            try
            {
                bool isTraineeDeleted = await _traineeServices.DeleteTraineeById(id);
                if (isTraineeDeleted)
                {
                    return NoContent();
                }
                else
                {
                    throw new Exception("Trainee Not Found");
                }
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
            
        }

    }
}
