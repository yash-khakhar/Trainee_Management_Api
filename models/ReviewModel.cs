using System.ComponentModel.DataAnnotations;
using TraineeManagement.api.DTO.ReviewDto;
using TraineeManagement.api.Enum;
using TraineeManagement.api.Repository.Review;

namespace TraineeManagement.api.Models
{
    public class ReviewModel : IReviewRepo
    {
        
        public ReviewModel(
            int submissionId, 
            int mentorId, 
            string feedback, 
            int score, 
            DateTime reviewDate,
            ReviewStatus status
        )
        {
            SubmissionId = submissionId;
            MentorId = mentorId;
            Feedback = feedback;
            Score = score;
            ReviewDate = reviewDate;
            Status = status;
        }
        
        [Key]
        public int Id { get; set; }

        [Required]
        public int SubmissionId { get; set; }

        [Required]
        public int MentorId { get; set; }

        [Required]
        public string Feedback { get; set; }
        public int Score { get; set; } = 0;

        [Required]
        public DateTime ReviewDate { get; set; }

        [Required]
        public ReviewStatus Status { get; set; }

        public SubmissionModel? Submission { get; set; }
        public MentorModel? Mentor { get; set; }

        public static ReviewResponse ToDto(ReviewModel reviewModel)
        {
            return new ReviewResponse
                (
                    reviewModel.Id,
                    reviewModel.SubmissionId,
                    reviewModel.MentorId,
                    reviewModel.Feedback,
                    reviewModel.Score,
                    reviewModel.ReviewDate,
                    reviewModel.Status
                );
        }

    }
}
