using System.ComponentModel.DataAnnotations;
using TraineeManagement.api.Enum.Task;

namespace TraineeManagement.api.DTO.Task
{
    public class CreateTaskRequest
    {
        [Required]
        public required string Title { get; set; }

        [Required]
        public required string Description { get; set; }

        [Required]
        public required string ExpectedTechStack { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        public TaskStatusEnum Status { get; set; }
    }
}
