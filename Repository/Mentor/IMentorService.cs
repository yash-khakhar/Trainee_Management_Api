using TraineeManagement.api.DTO.MentorDto;

namespace TraineeManagement.api.Repository.Mentor
{
    public interface IMentorService
    {
        public Task<IEnumerable<MentorResponse>> GetMentorList();
        public Task<MentorResponse> GetMentorById(int id);
        public Task<MentorResponse> AddMentor(CreateMentorRequest mentor);
        public Task<MentorResponse> UpdateMentor(int id, UpdateMentorRequest updateMentorRequest);
        public Task<bool> DeleteMentorById(int id);
    }
}
