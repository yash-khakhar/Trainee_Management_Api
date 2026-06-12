using TraineeManagement.api.DTO.TraineeDto;
using TraineeManagement.api.Enum;

namespace TraineeManagement.api.Repository.Trainee
{
    public interface ITraineeService
    {
        public Task<IEnumerable<TraineeResponse>> GetTraineeList();
        public Task<TraineeResponse> GetTraineeById(int id);
        public Task<TraineeResponse> AddTrainee(CreateTraineeRequest trainee);
        public Task<TraineeResponse> UpdateTrainee(UpdateTraineeRequest updateTraineeRequest);
        public Task<bool> DeleteTraineeById(int id);
        public Task<IEnumerable<TraineeResponse>> SearchTrainee(string searchKeyword);
        public Task<TraineeSearchResultDto> SearchWithPagination(int pageNumber, int pageSize, string search, TraineeStatusEnum role);

    }
}
