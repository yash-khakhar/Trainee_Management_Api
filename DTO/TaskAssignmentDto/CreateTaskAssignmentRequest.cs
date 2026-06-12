using System.ComponentModel.DataAnnotations;
using TraineeManagement.api.Enum;

namespace TraineeManagement.api.DTO.TaskAssignmentDto
{
    public class CreateTaskAssignmentRequest
    {
        [Required]
        public int TraineeId { get; set; }

        [Required]
        public int MentorId { get; set; }

        [Required]
        public int TaskId { get; set; }

        [Required]
        public DateTime AssignedDate { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        public TaskAssignmentStatusEnum Status { get; set; }
        public string? Remarks { get; set; }
    }
}
