using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraineeManagement.api.CustomException;
using TraineeManagement.api.DTO.TraineeDto;
using TraineeManagement.api.Enum.Trainee;
using TraineeManagement.api.Enum.User;
using TraineeManagement.api.repository;

namespace TraineeManagement.api.Controllers
{
 
    [Route("api/[controller]")]
    [Authorize(Roles = $"{nameof(UserRolesEnum.ADMIN)}, {nameof(UserRolesEnum.MENTOR)}, {nameof(UserRolesEnum.TRAINEE)}")]
    [ApiController]
    public class TraineeController : ControllerBase
    {

        private readonly ITraineeService _traineeServices;
        private readonly ILogger<TraineeController> _logger;
        public TraineeController(ITraineeService traineeServices, ILogger<TraineeController> logger)
        {
            _traineeServices = traineeServices;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> ListAllTrainee(
            [FromQuery(Name = "pageNumber")] int pageNumber = 0,
            [FromQuery(Name = "pageSize")] int pageSize = 0,
            [FromQuery(Name = "search")] string? search = null,
            [FromQuery(Name = "status")] TraineeStatusEnum status = TraineeStatusEnum.ACTIVE
        )
        {
            try
            {
                if (search != null && pageNumber != 0 && pageSize != 0)
                {
                    TraineeSearchResultDto traineeSearchResultDto = await _traineeServices.SearchWithPagination(pageNumber, pageSize, search.ToLower(), status);

                    return Ok(traineeSearchResultDto);

                }
                else if (search != null)
                {
                    return Ok(await _traineeServices.SearchTrainee(search.ToLower()));
                }
                else
                {
                    return Ok(await _traineeServices.GetTraineeList());
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception in Seaching Trainee: {ex.Message}");
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("{id}", Name = "GetTraineeById")]
        public async Task<ActionResult<TraineeResponse>> GetTraineeById(int id)
        {
            try
            {
                TraineeResponse trainee = await _traineeServices.GetTraineeById(id);
                return Ok(trainee);
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {

                var errorResponse = new { message = ex.Message };

                _logger.LogError($"ERROR: Exception in User Creation: {ex.Message}");

                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);

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
                TraineeResponse traineeRespone =  await _traineeServices.AddTrainee(trainee);

                _logger.LogInformation($"POST: New Trainee. {traineeRespone.FirstName} created!!");

                return traineeRespone;

            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR: Exception in Trainee Creation: {ex.Message}");
                return BadRequest(ex.Message);
            }

        }

        [HttpPut]
        [ProducesResponseType(typeof(TraineeResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<TraineeResponse>> UpdateTrainee([FromBody] UpdateTraineeRequest updateTraineeRequest)
        {

            try
            {
                TraineeResponse traineeRespone = await _traineeServices.UpdateTrainee(updateTraineeRequest);

                _logger.LogInformation($"PUT: Trainee Updation. {traineeRespone.FirstName} updated!!");

                return traineeRespone;
            }
            catch (Exception ex) 
            {
                _logger.LogError($"ERROR: Exception in Trainee Updation: {ex.Message}");
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
                    _logger.LogInformation($"Trainee Deleted: Trainee Id: {id}");
                    return NoContent();
                }
                else
                {
                    throw new Exception("Trainee Not Found");
                }
            }
            catch (Exception ex) 
            {
                _logger.LogError($"ERROR: Exception in Trainee Deletion: {ex.Message}");
                return BadRequest(ex.Message);
            }
            
        }

    }
}
