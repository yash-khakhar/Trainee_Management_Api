using System.ComponentModel.DataAnnotations;
using TraineeManagement.api.Enum;
using TraineeManagement.api.Repository.ProcessingJob;

namespace TraineeManagement.api.Models
{
    public class ProcessingJobModel : IProcessingJobRepo
    {

        public ProcessingJobModel(
            Guid messageId, 
            string correlationId, 
            int attempts, 
            string errorSummary,
            ProcessingJobStatusEnum status, 
            DateTime startDateTime
        )
        {
            MessageId = messageId;
            CorrelationId = correlationId;
            Attempts = attempts;
            ErrorSummary = errorSummary;
            Status = status;
            StartDateTime = startDateTime;
        }


        [Key]
        public int Id { get; set; }

        [Required]
        public Guid MessageId { get; set; }

        [Required]
        public string CorrelationId { get; set; }

        [Required]
        public int Attempts { get; set; }

        [Required]
        public string ErrorSummary { get; set; } = string.Empty;

        [Required]
        public ProcessingJobStatusEnum Status { get; set; }

        [Required]
        public DateTime StartDateTime { get; set; }
        public DateTime CompletedDateTime { get; set; }
    }
}
