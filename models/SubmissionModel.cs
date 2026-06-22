using System.ComponentModel.DataAnnotations;
using TraineeManagement.api.DTO.SubmissionDto;
using TraineeManagement.api.Enum;
using TraineeManagement.api.Repository.Submission;

namespace TraineeManagement.api.Models
{
    public class SubmissionModel : ISubmissionRepo
    {
        public SubmissionModel() { }

        public SubmissionModel(int taskAssignmentId, string submissionUrl, string notes, DateTime submittedDate, SubmissionStatusEnum status)
        {
            TaskAssignmentId = taskAssignmentId;
            SubmissionUrl = submissionUrl;
            Notes = notes;
            SubmittedDate = submittedDate;
            Status = status;
        }
        
        [Key]
        public int Id { get; set; }

        [Required]
        public int TaskAssignmentId { get; set; }

        [Required]
        public string SubmissionUrl { get; set; } = string.Empty;

        [Required]
        public string Notes { get; set; } = string.Empty;

        [Required]
        public DateTime SubmittedDate { get; set; }

        [Required]
        public SubmissionStatusEnum Status { get; set; }

        public TaskAssignmentModel? TaskAssignment { get; set; }

        public ICollection<SubmissionFileModel> SubmissionFiles { get; set; } = new List<SubmissionFileModel>();

        public static SubmissionResponse ToDto(SubmissionModel submissionModel)
        {
            return new SubmissionResponse
                (
                    submissionModel.Id,
                    submissionModel.TaskAssignmentId,
                    submissionModel.SubmissionUrl,
                    submissionModel.Notes,
                    submissionModel.SubmittedDate,
                    submissionModel.Status
                );
        }

    }
}
