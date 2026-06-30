using System.ComponentModel.DataAnnotations;
using TraineeManagement.api.Enum;

namespace TraineeManagement.api.DTO.ReviewDto
{
    public class CreateReviewRequest
    {
        [Required(ErrorMessage = "Submission Id is Required")]
        public int SubmissionId { get; set; }

        [Required(ErrorMessage = "Mentor Id is Required")]
        public int MentorId { get; set; }

        [Required(ErrorMessage = "Feedback is Required")]
        public required string Feedback { get; set; }

        public int Score { get; set; } = 0;

        [Required]
        public DateTime ReviewDate { get; set; }

        [Required]
        public ReviewStatus Status { get; set; }
    }
}
