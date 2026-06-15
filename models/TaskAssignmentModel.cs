using System.ComponentModel.DataAnnotations;
using TraineeManagement.api.DTO.TaskAssignmentDto;
using TraineeManagement.api.Enum;
using TraineeManagement.api.models;
using TraineeManagement.api.Repository.TaskAssignment;

namespace TraineeManagement.api.Models
{
    public class TaskAssignmentModel : ITaskAssignmentRepo
    {
        public TaskAssignmentModel(
            int traineeId,
            int mentorId,
            int taskId,
            DateTime assignedDate,
            DateTime dueDate,
            TaskAssignmentStatusEnum status,
            string remarks
        )
        {
            TraineeId = traineeId;
            MentorId = mentorId;
            TaskId = taskId;
            AssignedDate = assignedDate;
            DueDate = dueDate;
            Status = status;
            Remarks = remarks;

        }

        public int Id { get; set; }

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
        public TraineeModel? Trainee { get; set; }
        public MentorModel? Mentor { get; set; }
        public TaskModel? Task { get; set; }

        public static TaskAssignmentResponse ToDto(TaskAssignmentModel taskAssignmentModel)
        {
            return new TaskAssignmentResponse
                (
                    taskAssignmentModel.Id,
                    taskAssignmentModel.TraineeId,
                    taskAssignmentModel.MentorId,
                    taskAssignmentModel.TaskId,
                    taskAssignmentModel.AssignedDate,
                    taskAssignmentModel.DueDate,
                    taskAssignmentModel.Status,
                    taskAssignmentModel.Remarks == null ? string.Empty : taskAssignmentModel.Remarks
                );
        }

    }
}
