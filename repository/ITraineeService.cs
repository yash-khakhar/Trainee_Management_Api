using TraineeManagement.api.DTO.TraineeDto;

namespace TraineeManagement.api.repository
{
    public interface ITraineeService
    {
        public List<TraineeResponse> GetTraineeList();
        public TraineeResponse GetTraineeById(string id);
        public TraineeResponse AddTrainee(CreateTraineeRequest trainee);
        public TraineeResponse UpdateTrainee(UpdateTraineeRequest updateTraineeRequest);
        public void DeleteTraineeById(string id);

    }
}
