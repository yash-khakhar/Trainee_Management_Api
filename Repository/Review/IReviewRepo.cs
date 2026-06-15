using TraineeManagement.api.Enum;

namespace TraineeManagement.api.Repository.Review
{
    public interface IReviewRepo
    {
        int Id { get; set; }
        int SubmissionId { get; set; }
        int MentorId { get; set; }
        string Feedback { get; set; }
        int Score { get; set; }
        DateTime ReviewDate { get; set; }
        ReviewStatus Status { get; set; }
    }
}
