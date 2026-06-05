using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TraineeManagement.api.DTO.TraineeDto;
using TraineeManagement.api.models;
using TraineeManagement.api.repository;
using TraineeManagement.api.Services;

namespace TraineeManagement.api.Controllers
{
 
    [Route("api/[controller]")]
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
        public ActionResult<TraineeResponse[]> ListAllTrainee()
        {
            try
            {
                return Ok(_traineeServices.GetTraineeList());
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("{id}", Name = "GetTraineeById")]
        public ActionResult<TraineeResponse> GetTraineeById(string id)
        {
            try
            {
                return Ok(_traineeServices.GetTraineeById(id));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(TraineeResponse), StatusCodes.Status201Created)]
        public ActionResult<TraineeResponse> AddTrainee([FromBody] CreateTraineeRequest trainee)
        {

            if (trainee == null)
            {
                return BadRequest();
            }

            try
            {
                return _traineeServices.AddTrainee(trainee);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut]
        [ProducesResponseType(typeof(TraineeResponse), StatusCodes.Status200OK)]
        public ActionResult<TraineeResponse> UpdateTrainee([FromBody] UpdateTraineeRequest updateTraineeRequest)
        {

            try
            {
                return Ok(_traineeServices.UpdateTrainee(updateTraineeRequest));
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTraineeById(string id)
        {
            try
            {
                _traineeServices.DeleteTraineeById(id);
                return NoContent();
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
            
        }

    }
}
