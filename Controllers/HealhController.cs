using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TraineeManagement.api.models;

namespace TraineeManagement.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealhController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(HealthModel), StatusCodes.Status200OK)]
        public ActionResult<HealthModel> GetHealthResult()
        {
            var statusPayload = new HealthModel
            {
                Status = "Healthy",
                ApplicationName = "TraineeManagement.api",
                Time = DateTime.UtcNow
            };

            return Ok(statusPayload);

        }
    }
}
