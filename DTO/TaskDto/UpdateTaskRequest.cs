using TraineeManagement.api.Enum;

namespace TraineeManagement.api.DTO.Task
{
    public class UpdateTaskRequest
    {

        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ExpectedTechStack { get; set; }
        public DateTime? DueDate { get; set; }
        public TaskStatusEnum? Status { get; set; }
    }
}
