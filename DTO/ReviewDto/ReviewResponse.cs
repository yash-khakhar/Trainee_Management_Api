using TraineeManagement.api.Enum;

namespace TraineeManagement.api.DTO.ReviewDto
{
    public class ReviewResponse
    {
       
        public ReviewResponse(
            int id, 
            int submissionId, 
            int mentorId, 
            string feedback, 
            int score, 
            DateTime reviewDate, 
            ReviewStatus status
        )
        {
            Id = id;
            SubmissionId = submissionId;
            MentorId = mentorId;
            Feedback = feedback;
            Score = score;
            ReviewDate = reviewDate;
            Status = status;
        }

        public int Id { get; set; }
        public int SubmissionId { get; set; }
        public int MentorId { get; set; }
        public string Feedback { get; set; }
        public int Score { get; set; }
        public DateTime ReviewDate { get; set; }
        public ReviewStatus Status { get; set; }
    }
}
