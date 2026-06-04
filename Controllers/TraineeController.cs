using Microsoft.AspNetCore.Mvc;
using TraineeManagement.api.models;

namespace TraineeManagement.api.Controllers
{
 
    [Route("api/[controller]")]
    [ApiController]
    public class TraineeController : ControllerBase
    {
        private static readonly List<TraineeModel> traineeList = new List<TraineeModel>();

        [HttpGet]
        [ProducesResponseType(typeof(TraineeModel[]), StatusCodes.Status200OK)]
        public ActionResult<TraineeModel[]> ListAllTrainee()
        {
            return Ok(traineeList);

        }

        [HttpGet("{id}", Name = "GetTraineeById")]
        public ActionResult<TraineeModel> GetTraineeById(int id)
        {
            var trainee = traineeList.FirstOrDefault(t => t.Id == id);
            if (trainee == null) return NotFound();
            return Ok(trainee);
        }

        [HttpPost]
        [ProducesResponseType(typeof(TraineeModel), StatusCodes.Status201Created)]
        public ActionResult<TraineeModel> AddTrainee([FromBody] TraineeModel trainee)
        {   

            if(trainee == null)
            {
                return BadRequest();
            }

            trainee.Id = traineeList.Count + 1;
            trainee.CreatedAt = DateTime.UtcNow;
            trainee.UpdatedAt = DateTime.UtcNow;
            traineeList.Add(trainee);

            return CreatedAtAction(
                nameof(GetTraineeById),
                new { id = trainee.Id },
                trainee
            );

        }


    }
}
