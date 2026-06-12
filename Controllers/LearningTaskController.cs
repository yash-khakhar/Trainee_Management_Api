using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraineeManagement.api.DTO.Task;
using TraineeManagement.api.Enum;
using TraineeManagement.api.Repository.Task;

namespace TraineeManagement.api.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = $"{nameof(UserRolesEnum.ADMIN)}, {nameof(UserRolesEnum.MENTOR)}")]
    [ApiController]
    public class LearningTaskController : ControllerBase
    {

        private readonly ITaskService _taskService;
        private readonly ILogger<LearningTaskController> _logger;
        public LearningTaskController(ITaskService taskService, ILogger<LearningTaskController> logger)
        {
            _taskService = taskService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTasks()
        {
            IEnumerable<TaskResponse> taskList = await _taskService.GetTaskList();
            return Ok(taskList);
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            TaskResponse task = await _taskService.GetTaskById(id);
            return Ok(task);
        }

        
        [HttpPost]
        public async Task<IActionResult> AddTask([FromBody] CreateTaskRequest taskRequest)
        {
            TaskResponse task = await _taskService.AddTask(taskRequest);
            _logger.LogInformation($"NEW TASK ADDED: Task Name: {taskRequest.Title}");
            return StatusCode(StatusCodes.Status201Created, task);
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] UpdateTaskRequest taskRequest)
        {
            TaskResponse task = await _taskService.UpdateTask(id, taskRequest);
            _logger.LogInformation($"TASK UPDATED: Task Name: {taskRequest.Title}");
            return StatusCode(StatusCodes.Status200OK, task);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            bool isTaskDeleted = await _taskService.DeleteTaskById(id);
            _logger.LogInformation($"TASK DELETED: Task Id: {id}");
            return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}
