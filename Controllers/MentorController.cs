using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraineeManagement.api.DTO.MentorDto;
using TraineeManagement.api.Enum;
using TraineeManagement.api.Repository.Mentor;

namespace TraineeManagement.api.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = $"{nameof(UserRolesEnum.ADMIN)}")]
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
            IEnumerable<MentorResponse> mentorList = await _mentorService.GetMentorList();
            return Ok(mentorList);
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMentorById(int id)
        {
            MentorResponse mentor = await _mentorService.GetMentorById(id);
            return Ok(mentor);
        }

        
        [HttpPost]
        public async Task<IActionResult> AddMentor([FromBody] CreateMentorRequest mentorRequest)
        {
            MentorResponse mentor = await _mentorService.AddMentor(mentorRequest);

            _logger.LogInformation($"NEW MENTOR ADDED: Mentor Name: {mentorRequest.FirstName}");

            return StatusCode(StatusCodes.Status201Created, mentor);
        }

       
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMentor([FromRoute]int id, [FromBody] UpdateMentorRequest mentorRequest)
        {
            MentorResponse mentor = await _mentorService.UpdateMentor(id, mentorRequest);

            _logger.LogInformation($"MENTOR UPDATED: Mentor Name: {mentorRequest.FirstName}");

            return StatusCode(StatusCodes.Status200OK, mentor);

        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMentor(int id)
        {
            bool isMentorDeleted = await _mentorService.DeleteMentorById(id);

            _logger.LogInformation($"MENTOR DELETED: Mentor Id: {id}");

            return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}
