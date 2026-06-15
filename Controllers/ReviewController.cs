using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraineeManagement.api.DTO.ReviewDto;
using TraineeManagement.api.Enum;
using TraineeManagement.api.Repository.Review;

namespace TraineeManagement.api.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = $"{nameof(UserRolesEnum.ADMIN)}, {nameof(UserRolesEnum.MENTOR)}")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        private readonly ILogger<ReviewController> _logger;

        public ReviewController(IReviewService reviewService, ILogger<ReviewController> logger)
        {
            _reviewService = reviewService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllReviews()
        {
            IEnumerable<ReviewResponse> reviewList = await _reviewService.GetReviewList();
            return Ok(reviewList);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetReviewById(int id)
        {

            if (id <= 0) throw new Exception("Invalid Data Input");

            ReviewResponse review = await _reviewService.GetReviewById(id);
            return Ok(review);
        }


        [HttpPost]
        public async Task<IActionResult> AddReview([FromBody] CreateReviewRequest reviewRequest)
        {
            if (reviewRequest == null) throw new Exception("Invalid Data Input");

            ReviewResponse review = await _reviewService.AddReview(reviewRequest);
            _logger.LogInformation($"NEW Review ADDED");
            return StatusCode(StatusCodes.Status201Created, review);
        }
    }

}
