using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraineeManagement.api.DTO.TaskAssignmentDto;
using TraineeManagement.api.Enum;
using TraineeManagement.api.Repository.TaskAssignment;
namespace TraineeManagement.api.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = $"{nameof(UserRolesEnum.ADMIN)}, {nameof(UserRolesEnum.MENTOR)}")]
    [ApiController]
    public class TaskAssignmentController : ControllerBase
    {

        private readonly ITaskAssignmentService _taskAssignmentService;
        private readonly ILogger<TaskAssignmentController> _logger;

        public TaskAssignmentController(ITaskAssignmentService taskAssignmentService, ILogger<TaskAssignmentController> logger)
        {
            _taskAssignmentService = taskAssignmentService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTaskAssignments()
        {
            IEnumerable<TaskAssignmentResponse> taskList = await _taskAssignmentService.GetTaskAssignmentList();
            return Ok(taskList);
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskAssignmentById(int id)
        {
            TaskAssignmentResponse task = await _taskAssignmentService.GetTaskAssignmentById(id);
            return Ok(task);
        }

       
        [HttpPost]
        public async Task<IActionResult> AddTaskAssignment([FromBody] CreateTaskAssignmentRequest taskRequest)
        {
            TaskAssignmentResponse task = await _taskAssignmentService.AddTaskAssignment(taskRequest);
            _logger.LogInformation($"NEW TASK Assignment ADDED");
            return StatusCode(StatusCodes.Status201Created, task);
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTaskAssignment(int id, [FromBody] UpdateTaskAssignmentRequest taskRequest)
        {
            TaskAssignmentResponse task = await _taskAssignmentService.UpdateTaskAssignment(id, taskRequest);
            _logger.LogInformation($"TASK Assignment UPDATED");
            return StatusCode(StatusCodes.Status200OK, task);
        }

    }
}
