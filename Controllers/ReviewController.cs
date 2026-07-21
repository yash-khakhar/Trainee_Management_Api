using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraineeManagement.api.CustomException;
using TraineeManagement.api.DTO.ReviewDto;
using TraineeManagement.api.Enum;
using TraineeManagement.api.Repository.Review;

namespace TraineeManagement.api.Controllers
{
    [Route("api/[controller]")]
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
        [Authorize(Roles = $"{nameof(UserRolesEnum.ADMIN)}")]
        public async Task<IActionResult> GetAllReviews()
        {
            IEnumerable<ReviewResponse> reviewList = await _reviewService.GetReviewList();
            return Ok(reviewList);
        }


        [HttpGet("{id}")]
        [Authorize(Roles = $"{nameof(UserRolesEnum.ADMIN)}, {nameof(UserRolesEnum.MENTOR)}, {nameof(UserRolesEnum.TRAINEE)}")]
        public async Task<IActionResult> GetReviewById(int id)
        {

            if (id <= 0) throw new InvalidRequest("Invalid Data Input");

            ReviewResponse review = await _reviewService.GetReviewById(id);
            return Ok(review);
        }


        [HttpPost]
        [Authorize(Roles = $"{nameof(UserRolesEnum.MENTOR)}")]
        public async Task<IActionResult> AddReview([FromBody] CreateReviewRequest reviewRequest)
        {
            if (reviewRequest == null) throw new InvalidRequest("Invalid Data Input");

            ReviewResponse review = await _reviewService.AddReview(reviewRequest);
            _logger.LogInformation($"NEW Review ADDED");
            return StatusCode(StatusCodes.Status201Created, review);
        }
    }

}
