using TraineeManagement.api.Enum;

namespace TraineeManagement.api.DTO.SubmissionDto
{
    public class SubmissionResponse
    {
        public SubmissionResponse(
            int id, 
            int taskAssignmentId, 
            string submissionUrl, 
            string notes, 
            DateTime submittedDate, 
            SubmissionStatusEnum status
        )
        {
            Id = id;
            TaskAssignmentId = taskAssignmentId;
            SubmissionUrl = submissionUrl;
            Notes = notes;
            SubmittedDate = submittedDate;
            Status = status;
        }

        public SubmissionResponse() {
            
        }
        
        public int Id { get; set; }
        public int TaskAssignmentId { get; set; }
        public string SubmissionUrl { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public DateTime SubmittedDate { get; set; }
        public SubmissionStatusEnum Status { get; set; }

    }
}
