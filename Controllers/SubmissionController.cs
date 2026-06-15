using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraineeManagement.api.DTO.SubmissionDto;
using TraineeManagement.api.Enum;
using TraineeManagement.api.Repository.Submission;

namespace TraineeManagement.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = $"{nameof(UserRolesEnum.ADMIN)}, {nameof(UserRolesEnum.MENTOR)}")]
    public class SubmissionController : ControllerBase
    {
        private readonly ISubmissionService _submissionService;
        private readonly ILogger<SubmissionController> _logger;

        public SubmissionController(ISubmissionService submissionService, ILogger<SubmissionController> logger)
        {
            _submissionService = submissionService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSubmissions()
        {
            IEnumerable<SubmissionResponse> submissionList = await _submissionService.GetSubmissionList();
            return Ok(submissionList);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetSubmissionById(int id)
        {

            if (id <= 0) throw new Exception("Invalid Data Input");

            SubmissionResponse submission = await _submissionService.GetSubmissionById(id);
            return Ok(submission);
        }


        [HttpPost]
        [Authorize(Roles = $"{nameof(UserRolesEnum.TRAINEE)}")]
        public async Task<IActionResult> AddSubmission([FromBody] CreateSubmissionRequest submissionRequest)
        {
            if (submissionRequest == null) throw new Exception("Invalid Data Input");

            SubmissionResponse submission = await _submissionService.AddSubmission(submissionRequest);
            _logger.LogInformation($"NEW Submission ADDED");
            return StatusCode(StatusCodes.Status201Created, submission);
        }

    }
}
