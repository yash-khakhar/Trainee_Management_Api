using Microsoft.EntityFrameworkCore;
using TraineeManagement.api.CustomException;
using TraineeManagement.api.Data;
using TraineeManagement.api.DTO.ReviewDto;
using TraineeManagement.api.Models;
using TraineeManagement.api.Repository.Review;

namespace TraineeManagement.api.Services
{
    public class ReviewService : IReviewService
    {
        private AppDbContext _context;

        public ReviewService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ReviewResponse> AddReview(CreateReviewRequest reviewRequest)
        {

            ReviewModel reviewModel = new ReviewModel(
                reviewRequest.SubmissionId,
                reviewRequest.MentorId,
                reviewRequest.Feedback,
                reviewRequest.Score,
                reviewRequest.ReviewDate,
                reviewRequest.Status
            );


            _context.Review.Add(reviewModel);

            await _context.SaveChangesAsync();

            return ReviewModel.ToDto(reviewModel);
        }

        public async Task<ReviewResponse> GetReviewById(int id)
        {
            ReviewResponse? reviewResponse = await _context.Review
                .Where(s => s.Id == id)
                .Select(s => new ReviewResponse(
                    s.Id, 
                    s.SubmissionId, 
                    s.MentorId, 
                    s.Feedback, 
                    s.Score, 
                    s.ReviewDate, 
                    s.Status
                ))
                .FirstOrDefaultAsync();

            if (reviewResponse == null) throw new NotFoundException("Review Not Found");

            return reviewResponse;
        }

        public async Task<IEnumerable<ReviewResponse>> GetReviewList()
        {
            List<ReviewResponse> reviewResponse = await _context.Review
                .Select(s => new ReviewResponse(
                    s.Id, 
                    s.SubmissionId, 
                    s.MentorId, 
                    s.Feedback, 
                    s.Score, 
                    s.ReviewDate, 
                    s.Status
                ))
                .ToListAsync();

            return reviewResponse;
        }
    }
}
