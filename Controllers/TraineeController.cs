using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraineeManagement.api.CustomException;
using TraineeManagement.api.DTO.TraineeDto;
using TraineeManagement.api.Enum;
using TraineeManagement.api.Repository.Trainee;

namespace TraineeManagement.api.Controllers
{
 
    [Route("api/[controller]")]
    [Authorize(Roles = $"{nameof(UserRolesEnum.ADMIN)}, {nameof(UserRolesEnum.MENTOR)}")]
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

        [HttpGet("{id}", Name = "GetTraineeById")]
        public async Task<ActionResult<TraineeResponse>> GetTraineeById(int id)
        {
            TraineeResponse trainee = await _traineeServices.GetTraineeById(id);
            return Ok(trainee);
        }

        [HttpPost]
        [ProducesResponseType(typeof(TraineeResponse), StatusCodes.Status201Created)]
        public async Task<ActionResult<TraineeResponse>> AddTrainee([FromBody] CreateTraineeRequest trainee)
        {

            if (trainee == null)
            {
                throw new NotFoundException("Trainee Not Found");
            }

            TraineeResponse traineeRespone = await _traineeServices.AddTrainee(trainee);

            _logger.LogInformation($"POST: New Trainee. {traineeRespone.FirstName} created!!");

            return traineeRespone;

        }

        [HttpPut]
        [ProducesResponseType(typeof(TraineeResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<TraineeResponse>> UpdateTrainee([FromBody] UpdateTraineeRequest updateTraineeRequest)
        {

            TraineeResponse traineeRespone = await _traineeServices.UpdateTrainee(updateTraineeRequest);

            _logger.LogInformation($"PUT: Trainee Updation. {traineeRespone.FirstName} updated!!");

            return traineeRespone;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTraineeById(int id)
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

    }
}
