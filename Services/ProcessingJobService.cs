using Microsoft.EntityFrameworkCore;
using TraineeManagement.api.CustomException;
using TraineeManagement.api.Data;
using TraineeManagement.api.DTO.ProcessingJobDto;
using TraineeManagement.api.Repository.ProcessingJob;

namespace TraineeManagement.api.Services
{
    public class ProcessingJobService : IProcessingJobService
    {
        private AppDbContext _context;
        public ProcessingJobService(AppDbContext context)
        {
            _context = context;
        }

        
        public async Task<ProcessingJobResponse> GetProcessingJobDetails(int id)
        {
            ProcessingJobResponse? response = await _context.ProcessingJob
                .Where(job => job.Id == id)
                .Select(job => new ProcessingJobResponse(job.Id, job.CorrelationId, job.MessageId, job.Attempts, job.ErrorSummary, job.Status, job.StartDateTime, job.CompletedDateTime))
                .FirstOrDefaultAsync();

            if (response == null) throw new NotFoundException($"Processing Job Not Found: Job Id: {id}");

            return response;

        }
    }
}
