using TraineeManagement.api.Enum;

namespace TraineeManagement.api.DTO.TaskAssignmentDto
{
    public class TaskAssignmentResponse
    {

        public TaskAssignmentResponse(
            int id,
            int traineeId,
            int mentorId,
            int taskId,
            DateTime assignedDate,
            DateTime dueDate,
            TaskAssignmentStatusEnum status,
            string? remarks
        )
        {
            Id = id;
            TraineeId = traineeId;
            MentorId = mentorId;
            TaskId = taskId;
            AssignedDate = assignedDate;
            DueDate = dueDate;
            Status = status;
            Remarks = remarks;
        }

        public int Id { get; set; }
        public int TraineeId { get; set; }
        public int MentorId { get; set; }
        public int TaskId { get; set; }
        public DateTime AssignedDate { get; set; }
        public DateTime DueDate { get; set; }
        public TaskAssignmentStatusEnum Status { get; set; }
        public string? Remarks { get; set; }
    }
}
