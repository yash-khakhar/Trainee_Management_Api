using TraineeManagement.api.Enum;

namespace TraineeManagement.api.Repository.ProcessingJob
{
    public interface IProcessingJobRepo
    {
        int Id { get; set; }

        Guid MessageId { get; set; }

        string CorrelationId { get; set; }

        int Attempts { get; set; }

        string ErrorSummary { get; set; }

        ProcessingJobStatusEnum Status { get; set; }

        DateTime StartDateTime { get; set; }

        DateTime CompletedDateTime { get; set; }

    }
}
