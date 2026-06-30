using System.ComponentModel.DataAnnotations;
using TraineeManagement.api.Enum;

namespace TraineeManagement.api.DTO.SubmissionDto
{
    public class CreateSubmissionRequest
    {
        [Required(ErrorMessage = "Task Assignment Id is Required")]
        public int TaskAssignmentId { get; set; }

        [Required(ErrorMessage = "Submission Url is Required")]
        public string SubmissionUrl { get; set; } = string.Empty;

        [Required(ErrorMessage = "Notes is Required")]
        public required string Notes { get; set; }

        [Required]
        public DateTime SubmittedDate { get; set; }

        [Required]
        public SubmissionStatusEnum Status { get; set; }
    }
}
