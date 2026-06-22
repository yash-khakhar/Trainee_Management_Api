using TraineeManagement.api.Enum;

namespace TraineeManagement.api.DTO.Task
{
    public class TaskResponse
    {
        public TaskResponse() { }

        public TaskResponse(
            int id, 
            string title, 
            string description, 
            string expectedTechStack, 
            DateTime dueDate, 
            TaskStatusEnum status, 
            DateTime createdAt, 
            DateTime updatedAt
        )
        {
            Id = id;
            Title = title;
            Description = description;
            ExpectedTechStack = expectedTechStack;
            DueDate = dueDate;
            Status = status;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ExpectedTechStack { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public TaskStatusEnum Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
