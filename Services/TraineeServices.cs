using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TraineeManagement.api.Data;
using TraineeManagement.api.DTO.TraineeDto;
using TraineeManagement.api.Enum.Trainee;
using TraineeManagement.api.Enum.User;
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
            TraineeModel? trainee = await _context.Trainees.FindAsync(id);
            if (trainee == null) throw new Exception("Trainee Not Found");
            else return trainee;
        }

        public async Task<TraineeResponse> AddTrainee(CreateTraineeRequest trainee)
        {
            TraineeModel traineeModel = new TraineeModel(
                    trainee.FirstName.ToLower(), 
                    trainee.LastName.ToLower(), 
                    trainee.Email.ToLower(), 
                    trainee.TechStack.ToLower(), 
                    trainee.Status
                );
            traineeModel.CreatedAt = DateTime.UtcNow;
            traineeModel.UpdatedAt = DateTime.UtcNow;
            
            _context.Trainees.Add(traineeModel);
            await _context.SaveChangesAsync();

            return TraineeModel.ToDto(traineeModel);
        }

        public async Task<bool> DeleteTraineeById(int id)
        {
            TraineeModel? trainee = await _context.Trainees.FindAsync(id);
            if (trainee == null) return false;
            _context.Trainees.Remove(trainee);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<TraineeResponse> GetTraineeById(int id)
        {
 
            TraineeResponse? trainee = await _context.Trainees
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

        public async Task<IEnumerable<TraineeResponse>> SearchTrainee(string searchKeyword)
        {
            var traineeList = await _context.Trainees
                .Where(
                    trainee => trainee.FirstName.Contains(searchKeyword) || 
                    trainee.LastName.Contains(searchKeyword) || 
                    trainee.Email.Contains(searchKeyword) || 
                    trainee.TechStack.Contains(searchKeyword
                ))
                .Select(trainee => 
                    new TraineeResponse(
                        trainee.Id, 
                        trainee.FirstName, 
                        trainee.LastName, 
                        trainee.Email, 
                        trainee.TechStack,
                        trainee.Status,
                        trainee.CreatedAt,
                        trainee.UpdatedAt
                     ))
                .ToListAsync();

            return traineeList;
        }

        public async Task<TraineeSearchResultDto> SearchWithPagination(int pageNumber, int pageSize, string search, TraineeStatusEnum role)
        {

            int totalRecords = await _context.Trainees.CountAsync();

            int totalPages = (totalRecords + pageSize - 1) / pageSize;

            int skip = pageSize * pageNumber - pageSize;

            var traineeList = await _context.Trainees
                .Skip(skip)
                .Take(pageSize)
                .Where(
                    trainee => trainee.FirstName.Contains(search) &&
                    trainee.Status.Equals(role)
                )
                .Select(trainee =>
                    new TraineeResponse(
                        trainee.Id,
                        trainee.FirstName,
                        trainee.LastName,
                        trainee.Email,
                        trainee.TechStack,
                        trainee.Status,
                        trainee.CreatedAt,
                        trainee.UpdatedAt
                        )
                ).ToListAsync();

            return new TraineeSearchResultDto(pageNumber, pageSize, totalRecords, traineeList);

        }
    }
}
