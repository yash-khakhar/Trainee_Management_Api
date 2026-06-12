using System.ComponentModel.DataAnnotations;
using TraineeManagement.api.DTO.Task;
using TraineeManagement.api.Enum;
using TraineeManagement.api.Repository.Task;

namespace TraineeManagement.api.Models
{
    public class TaskModel : ITaskRepo
    {
        public TaskModel(string title, string description, string expectedTechStack, DateTime dueDate, TaskStatusEnum status)
        {
            Title = title;
            Description = description;
            ExpectedTechStack = expectedTechStack;
            DueDate = dueDate;
            Status = status;
        }

        [Key]
        public int Id { get; set; }
        
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string ExpectedTechStack { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        public TaskStatusEnum Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public static TaskResponse ToDto(TaskModel taskModel)
        {
            return new TaskResponse
                (
                    taskModel.Id,
                    taskModel.Title,
                    taskModel.Description,
                    taskModel.ExpectedTechStack, 
                    taskModel.DueDate,
                    taskModel.Status,
                    taskModel.CreatedAt,
                    taskModel.UpdatedAt
                );
        }
    }
}
