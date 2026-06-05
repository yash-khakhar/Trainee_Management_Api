using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TraineeManagement.api.Data;
using TraineeManagement.api.DTO.TraineeDto;
using TraineeManagement.api.models;
using TraineeManagement.api.repository;

namespace TraineeManagement.api.Services
{
    public class TraineeServices : ITraineeService
    {
        private AppDbContext _context;

        public TraineeServices(AppDbContext context)
        {
            _context = context;
        }

        private async Task<TraineeModel> FindTraineeById(int id)
        {
            var trainee = await _context.Trainees.FindAsync(id);
            if (trainee == null) throw new Exception("Trainee Not Found");
            else return trainee;
        }

        public async Task<TraineeResponse> AddTrainee(CreateTraineeRequest trainee)
        {
            var traineeModel = new TraineeModel(trainee.FirstName, trainee.LastName, trainee.Email, trainee.TechStack, trainee.Status);
            traineeModel.CreatedAt = DateTime.UtcNow;
            traineeModel.UpdatedAt = DateTime.UtcNow;
            
            _context.Trainees.Add(traineeModel);
            await _context.SaveChangesAsync();

            return TraineeModel.ToDto(traineeModel);
        }

        public async void DeleteTraineeById(int id)
        {
            var trainee = await FindTraineeById(id);
            _context.Trainees.Remove(trainee);
            await _context.SaveChangesAsync();

        }

        public async Task<TraineeResponse> GetTraineeById(int id)
        {
 
            var trainee = await _context.Trainees
             .Where(p => p.Id == id)
            .Select(p => new TraineeResponse(p.Id, p.FirstName, p.LastName, p.Email, p.TechStack, p.Status, p.CreatedAt, p.UpdatedAt))
             .FirstOrDefaultAsync();

            if (trainee == null) throw new Exception("Trainee Not Found");

            return trainee;
            
        }

        public async Task<IEnumerable<TraineeResponse>> GetTraineeList()
        {
            var traineeList = await _context.Trainees
            .Select(p => new TraineeResponse(p.Id, p.FirstName, p.LastName, p.Email, p.TechStack, p.Status, p.CreatedAt, p.UpdatedAt))
            .ToListAsync();

            return traineeList;
        }

        public async Task<TraineeResponse> UpdateTrainee(UpdateTraineeRequest updateTraineeRequest)
        {
            var trainee = await FindTraineeById(updateTraineeRequest.Id);

            if (updateTraineeRequest.FirstName != null) trainee.FirstName = updateTraineeRequest.FirstName;

            if (updateTraineeRequest.LastName != null) trainee.LastName = updateTraineeRequest.LastName;

            if (updateTraineeRequest.Email != null) trainee.Email = updateTraineeRequest.Email;

            trainee.Status = (Enum.Trainee.TraineeStatusEnum)updateTraineeRequest.Status;

            if (updateTraineeRequest.TechStack != null) trainee.TechStack = updateTraineeRequest.TechStack;

            trainee.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return TraineeModel.ToDto(trainee);
        }
    }
}
