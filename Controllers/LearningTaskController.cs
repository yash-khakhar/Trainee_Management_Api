using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraineeManagement.api.CustomException;
using TraineeManagement.api.DTO.Task;
using TraineeManagement.api.Enum.User;
using TraineeManagement.api.Repository;

namespace TraineeManagement.api.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = $"{nameof(UserRolesEnum.ADMIN)}")]
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
            try
            {
                IEnumerable<TaskResponse> taskList = await _taskService.GetTaskList();
                return Ok(taskList);
            }
            catch (Exception ex)
            {
                var errorResponse = new { message = ex.Message };

                _logger.LogError($"ERROR: Exception in fetching all tasks: {ex.Message}");

                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);

            }
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            try
            {
                TaskResponse task = await _taskService.GetTaskById(id);
                return Ok(task);
            }
            catch (Exception ex)
            {
                var errorResponse = new { message = ex.Message };

                _logger.LogError($"ERROR: Exception in fetching task by id {id}: {ex.Message}");

                if (ex is NotFoundException notFoundEx)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, errorResponse);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
                }

            }
        }

        
        [HttpPost]
        public async Task<IActionResult> AddTask([FromBody] CreateTaskRequest taskRequest)
        {
            try
            {
                TaskResponse task = await _taskService.AddTask(taskRequest);
                _logger.LogInformation($"NEW TASK ADDED: Task Name: {taskRequest.Title}");
                return StatusCode(StatusCodes.Status201Created, task);
            }
            catch (Exception ex)
            {
                var errorResponse = new { message = ex.Message };
                _logger.LogError($"ERROR: Exception in Adding Task: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] UpdateTaskRequest taskRequest)
        {
            try
            {
                TaskResponse task = await _taskService.UpdateTask(id, taskRequest);
                _logger.LogInformation($"TASK UPDATED: Task Name: {taskRequest.Title}");
                return StatusCode(StatusCodes.Status200OK, task);
            }
            catch (Exception ex)
            {
                var errorResponse = new { message = ex.Message };

                _logger.LogError($"ERROR: Exception in Updating Task: {ex.Message}");

                if (ex is NotFoundException notFoundEx)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, errorResponse);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
                }

            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            try
            {
                bool isTaskDeleted = await _taskService.DeleteTaskById(id);
                _logger.LogInformation($"TASK DELETED: Task Id: {id}");
                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (Exception ex)
            {
                var errorResponse = new { message = ex.Message };

                _logger.LogError($"ERROR: Exception in Deleting Task: {ex.Message}");

                if (ex is NotFoundException notFoundEx)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, errorResponse);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
                }

            }
        }
    }
}
