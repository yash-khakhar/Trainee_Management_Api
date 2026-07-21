using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TraineeManagement.api.CustomException;
using TraineeManagement.api.DTO.TraineeDto;
using TraineeManagement.api.Enum;
using TraineeManagement.api.HttpClientFactory;
using TraineeManagement.api.Repository.Trainee;

namespace TraineeManagement.api.Controllers
{
 
    [Route("api/[controller]")]
    [ApiController]
    public class TraineeController : ControllerBase
    {

        private readonly ITraineeService _traineeServices;
        private readonly ILogger<TraineeController> _logger;
        private readonly DummyTraineeService _dummyTraineeService;
        public TraineeController(ITraineeService traineeServices, ILogger<TraineeController> logger, DummyTraineeService dummyTraineeService)
        {
            _traineeServices = traineeServices;
            _logger = logger;
            _dummyTraineeService = dummyTraineeService;
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
        [Authorize(Roles = $"{nameof(UserRolesEnum.ADMIN)}")]
        public async Task<ActionResult<TraineeResponse>> AddTrainee([FromBody] CreateTraineeRequest trainee)
        {

            if (trainee == null)
            {
                throw new InvalidRequest("Invalid Request");
            }

            TraineeResponse traineeRespone = await _traineeServices.AddTrainee(trainee);

            _logger.LogInformation($"POST: New Trainee. {traineeRespone.FirstName} created!!");

            return traineeRespone;

        }

        [HttpPut]
        [ProducesResponseType(typeof(TraineeResponse), StatusCodes.Status200OK)]
        [Authorize(Roles = $"{nameof(UserRolesEnum.ADMIN)}, {nameof(UserRolesEnum.TRAINEE)}")]
        public async Task<ActionResult<TraineeResponse>> UpdateTrainee([FromBody] UpdateTraineeRequest updateTraineeRequest)
        {

            TraineeResponse traineeRespone = await _traineeServices.UpdateTrainee(updateTraineeRequest);

            _logger.LogInformation($"PUT: Trainee Updation. {traineeRespone.FirstName} updated!!");

            return traineeRespone;
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = $"{nameof(UserRolesEnum.ADMIN)}")]
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
                throw new NotFoundException("Trainee Not Found");
            }

        }

        [HttpGet("dummy-trainee/{id}")]
        [Authorize(Roles = $"{nameof(UserRolesEnum.ADMIN)}, {nameof(UserRolesEnum.TRAINEE)}")]
        public async Task<IActionResult> GetDummyTraineeData(int id, CancellationToken cancellationToken)
        {
            var outgoingCorrelationId = Activity.Current?.RootId
                              ?? HttpContext.TraceIdentifier;

            Console.WriteLine($"[CONSUMER API] Outgoing Request! Correlation ID: {outgoingCorrelationId}");

            var trainee = await _dummyTraineeService.GetTraineeById(id, cancellationToken);
            return Ok(trainee);

        }
    }
}
