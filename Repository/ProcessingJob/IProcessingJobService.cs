using TraineeManagement.api.DTO.ProcessingJobDto;

namespace TraineeManagement.api.Repository.ProcessingJob
{
    public interface IProcessingJobService
    {
        public Task<ProcessingJobResponse> GetProcessingJobDetails(int id);
    }
}
