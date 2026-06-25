using TraineeManagement.api.Enum;

namespace TraineeManagement.api.DTO.ProcessingJobDto
{
    public class ProcessingJobResponse
    {
        
        public ProcessingJobResponse(
            int id, 
            string correclationId,
            Guid messageId,
            int attempts,
            string errorSummary,
            ProcessingJobStatusEnum status,
            DateTime startDateTime,
            DateTime completedDateTime
        )
        {
            Id = id;
            CorrelationId = correclationId;
            MessageId = messageId;
            Attempts = attempts;
            ErrorSummary = errorSummary;
            Status = status;
            StartDateTime = startDateTime;
            CompletedDateTime = completedDateTime;
        }
        
        public int Id { get; set; }
        public string CorrelationId { get; set; } 
        public Guid MessageId { get; set; }
        public int Attempts { get; set; }
        public string ErrorSummary { get; set; } = string.Empty;
        public ProcessingJobStatusEnum Status { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime CompletedDateTime { get; set; }

    }
}
