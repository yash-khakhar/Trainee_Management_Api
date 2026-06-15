using System.ComponentModel.DataAnnotations;
using TraineeManagement.api.Enum;

namespace TraineeManagement.api.DTO.ReviewDto
{
    public class CreateReviewRequest
    {
        [Required]
        public int SubmissionId { get; set; }

        [Required]
        public int MentorId { get; set; }

        [Required]
        public required string Feedback { get; set; }

        [Required]
        public int Score { get; set; } = 0;

        [Required]
        public DateTime ReviewDate { get; set; }

        [Required]
        public ReviewStatus Status { get; set; }
    }
}
