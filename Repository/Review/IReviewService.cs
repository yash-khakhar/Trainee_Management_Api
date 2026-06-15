using TraineeManagement.api.DTO.ReviewDto;

namespace TraineeManagement.api.Repository.Review
{
    public interface IReviewService
    {
        public Task<IEnumerable<ReviewResponse>> GetReviewList();
        public Task<ReviewResponse> GetReviewById(int id);
        public Task<ReviewResponse> AddReview(CreateReviewRequest reviewRequest);
    }
}
