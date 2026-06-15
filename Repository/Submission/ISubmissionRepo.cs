using TraineeManagement.api.Enum;

namespace TraineeManagement.api.Repository.Submission
{
    public interface ISubmissionRepo
    {
        int Id { get; set; }
        int TaskAssignmentId { get; set; }
        string SubmissionUrl { get; set; }
        string Notes { get; set; }
        DateTime SubmittedDate { get; set; }
        SubmissionStatusEnum Status { get; set; }
    }
}
