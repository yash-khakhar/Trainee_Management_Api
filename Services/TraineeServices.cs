using TraineeManagement.api.DTO.TraineeDto;
using TraineeManagement.api.models;
using TraineeManagement.api.repository;

namespace TraineeManagement.api.Services
{
    public class TraineeServices : ITraineeService
    {
        private static readonly List<TraineeModel> traineeList = new List<TraineeModel>();

        private TraineeModel FindTraineeById(string id)
        {
            var trainee = traineeList.FirstOrDefault(t => t.Id.Equals(id));
            if (trainee == null) throw new Exception("Trainee Not Found");
            else return trainee;
        }

        public TraineeResponse AddTrainee(CreateTraineeRequest trainee)
        {
            var traineeModel = new TraineeModel(trainee.FirstName, trainee.LastName, trainee.Email, trainee.TechStack, trainee.Status);
            traineeModel.CreatedAt = DateTime.UtcNow;
            traineeModel.UpdatedAt = DateTime.UtcNow;
            traineeModel.Id = Guid.NewGuid().ToString();
            traineeList.Add(traineeModel);
            return TraineeModel.ToDto(traineeModel);
        }

        public void DeleteTraineeById(string id)
        {
            var trainee = FindTraineeById(id);
            traineeList.Remove(trainee);
        }

        public TraineeResponse GetTraineeById(string id)
        {
            var trainee = FindTraineeById(id);
            return TraineeModel.ToDto(trainee);
            
        }

        public List<TraineeResponse> GetTraineeList()
        {
            List<TraineeResponse> traineeResponseDto = new List<TraineeResponse>();

            foreach (TraineeModel model in traineeList)
            {
                traineeResponseDto.Add(TraineeModel.ToDto(model));
            }

            return traineeResponseDto;
        }

        public TraineeResponse UpdateTrainee(UpdateTraineeRequest updateTraineeRequest)
        {
            var trainee = FindTraineeById(updateTraineeRequest.Id);

            if (updateTraineeRequest.FirstName != null) trainee.FirstName = updateTraineeRequest.FirstName;

            if (updateTraineeRequest.LastName != null) trainee.LastName = updateTraineeRequest.LastName;

            if (updateTraineeRequest.Email != null) trainee.Email = updateTraineeRequest.Email;

            trainee.Status = (Enum.Trainee.TraineeStatusEnum)updateTraineeRequest.Status;

            if (updateTraineeRequest.TechStack != null) trainee.TechStack = updateTraineeRequest.TechStack;

            trainee.UpdatedAt = DateTime.UtcNow;

            return TraineeModel.ToDto(trainee);
        }
    }
}
