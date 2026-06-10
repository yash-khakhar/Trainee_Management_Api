using Microsoft.AspNetCore.Mvc;
using TraineeManagement.api.DTO.TraineeDto;

namespace TraineeManagement.api.repository
{
    public interface ITraineeService
    {
        public Task<IEnumerable<TraineeResponse>> GetTraineeList();
        public Task<TraineeResponse> GetTraineeById(int id);
        public Task<TraineeResponse> AddTrainee(CreateTraineeRequest trainee);
        public Task<TraineeResponse> UpdateTrainee(UpdateTraineeRequest updateTraineeRequest);
        public Task<bool> DeleteTraineeById(int id);
        public Task<IEnumerable<TraineeResponse>> SearchTrainee(string searchKeyword);

    }
}
