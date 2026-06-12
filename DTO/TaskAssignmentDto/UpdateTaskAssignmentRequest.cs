using TraineeManagement.api.Enum;

namespace TraineeManagement.api.DTO.TaskAssignmentDto
{
    public class UpdateTaskAssignmentRequest
    {
        public int? TraineeId { get; set; }
        public int? MentorId { get; set; }
        public int? TaskId { get; set; }
        public DateTime? AssignedDate { get; set; }
        public DateTime? DueDate { get; set; }
        public TaskAssignmentStatusEnum? Status { get; set; }
        public string? Remarks { get; set; }
    }
}
