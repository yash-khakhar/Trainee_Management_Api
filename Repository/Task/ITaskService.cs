using TraineeManagement.api.DTO.Task;

namespace TraineeManagement.api.Repository.Task
{
    public interface ITaskService
    {
        public Task<IEnumerable<TaskResponse>> GetTaskList();
        public Task<TaskResponse> GetTaskById(int id);
        public Task<TaskResponse> AddTask(CreateTaskRequest task);
        public Task<TaskResponse> UpdateTask(int id, UpdateTaskRequest task);
        public Task<bool> DeleteTaskById(int id);
    }
}
