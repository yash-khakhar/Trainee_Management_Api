using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraineeManagement.api.DTO.ProcessingJobDto;
using TraineeManagement.api.Enum;
using TraineeManagement.api.Repository.ProcessingJob;

namespace TraineeManagement.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = $"{nameof(UserRolesEnum.ADMIN)}")]
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
