using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using TraineeManagement.api.CustomException;
using TraineeManagement.api.DTO.MentorDto;
using TraineeManagement.api.Enum.User;
using TraineeManagement.api.Repository;

namespace TraineeManagement.api.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = $"{nameof(UserRolesEnum.ADMIN)}, {nameof(UserRolesEnum.MENTOR)}")]
    [ApiController]
    public class MentorController : ControllerBase
    {
        private readonly IMentorService _mentorService;
        private readonly ILogger<TraineeController> _logger;
        public MentorController(IMentorService mentorService, ILogger<TraineeController> logger)
        {
            _mentorService = mentorService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMentor()
        {
            try
            {
                IEnumerable<MentorResponse> mentorList = await _mentorService.GetMentorList();
                return Ok(mentorList);
            }
            catch (Exception ex)
            {
                var errorResponse = new { message = ex.Message };

                _logger.LogError($"ERROR: Exception in fetching all mentor: {ex.Message}");

                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);

            }
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMentorById(int id)
        {
            try
            {
                MentorResponse mentor = await _mentorService.GetMentorById(id);
                return Ok(mentor);
            }
            catch (Exception ex)
            {
                var errorResponse = new { message = ex.Message };

                _logger.LogError($"ERROR: Exception in fetching mentor by id {id}: {ex.Message}");

                if (ex is NotFoundException notFoundEx)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, errorResponse);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
                }

            }
        }

        
        [HttpPost]
        public async Task<IActionResult> AddMentor([FromBody] CreateMentorRequest mentorRequest)
        {
            try
            {
                MentorResponse mentor = await _mentorService.AddMentor(mentorRequest);

                _logger.LogInformation($"NEW MENTOR ADDED: Mentor Name: {mentorRequest.FirstName}");

                return StatusCode(StatusCodes.Status201Created, mentor);
            }
            catch (Exception ex)
            {
                var errorResponse = new { message = ex.Message };
                _logger.LogError($"ERROR: Exception in Adding Mentor: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }

       
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMentor([FromRoute]int id, [FromBody] UpdateMentorRequest mentorRequest)
        {
            try
            {
                MentorResponse mentor = await _mentorService.UpdateMentor(id, mentorRequest);

                _logger.LogInformation($"MENTOR UPDATED: Mentor Name: {mentorRequest.FirstName}");

                return StatusCode(StatusCodes.Status200OK, mentor);
            }
            catch (Exception ex)
            {
                var errorResponse = new { message = ex.Message };

                _logger.LogError($"ERROR: Exception in Updating Mentor: {ex.Message}");

                if (ex is NotFoundException notFoundEx)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, errorResponse);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
                }

            }
            
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMentor(int id)
        {
            try
            {
                bool isMentorDeleted = await _mentorService.DeleteMentorById(id);

                _logger.LogInformation($"MENTOR DELETED: Mentor Id: {id}");

                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (Exception ex)
            {
                var errorResponse = new { message = ex.Message };

                _logger.LogError($"ERROR: Exception in Deleting Mentor: {ex.Message}");

                if (ex is NotFoundException notFoundEx)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, errorResponse);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
                }

            }
        }
    }
}
