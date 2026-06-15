using TraineeManagement.api.Enum;

namespace TraineeManagement.api.DTO.TaskAssignmentDto
{
    public class UpdateTaskAssignmentRequest
    {
        public int? TraineeId { get; set; } = null;
        public int? MentorId { get; set; } = null;
        public int? TaskId { get; set; } = null;
        public DateTime? AssignedDate { get; set; } = null;
        public DateTime? DueDate { get; set; } = null;
        public TaskAssignmentStatusEnum? Status { get; set; } = null;
        public string? Remarks { get; set; } = null;
    }
}
