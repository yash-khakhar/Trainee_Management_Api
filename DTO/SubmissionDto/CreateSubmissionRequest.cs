using System.ComponentModel.DataAnnotations;
using TraineeManagement.api.Enum;

namespace TraineeManagement.api.DTO.SubmissionDto
{
    public class CreateSubmissionRequest
    {
        [Required]
        public int TaskAssignmentId { get; set; }

        [Required]
        public string SubmissionUrl { get; set; } = string.Empty;

        [Required]
        public required string Notes { get; set; }

        [Required]
        public DateTime SubmittedDate { get; set; }

        [Required]
        public SubmissionStatusEnum Status { get; set; }
    }
}
