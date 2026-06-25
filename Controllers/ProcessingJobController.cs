using Microsoft.AspNetCore.Mvc;
using TraineeManagement.api.DTO.ProcessingJobDto;
using TraineeManagement.api.Repository.ProcessingJob;

namespace TraineeManagement.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProcessingJobController : ControllerBase
    {

        private readonly IProcessingJobService _processingJobService;

        public ProcessingJobController(IProcessingJobService processingJobService) 
        {
            _processingJobService = processingJobService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetJobById(int id)
        {
            ProcessingJobResponse task = await _processingJobService.GetProcessingJobDetails(id);
            return Ok(task);
        }
    }
}
