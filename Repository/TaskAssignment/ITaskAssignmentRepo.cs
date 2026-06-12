using TraineeManagement.api.Enum;

namespace TraineeManagement.api.Repository.TaskAssignment
{
    public interface ITaskAssignmentRepo
    {
        int Id { get; set; }
        int TraineeId { get; set; }
        int MentorId { get; set; }
        int TaskId { get; set; }
        DateTime AssignedDate { get; set; }
        DateTime DueDate { get; set; }
        TaskAssignmentStatusEnum Status { get; set; }
        string? Remarks { get; set; }

    }
}
