using TraineeManagement.api.DTO.SubmissionDto;

namespace TraineeManagement.api.Repository.Submission
{
    public interface ISubmissionService
    {
        public Task<IEnumerable<SubmissionResponse>> GetSubmissionList();
        public Task<SubmissionResponse> GetSubmissionById(int id);
        public Task<SubmissionResponse> AddSubmission(CreateSubmissionRequest submissionRequest, List<IFormFile> files);

        public Task<(byte[] FileBytes, string ContentType, string FileName)> DownloadFileAsync(int fileMetadataId);
        public Task<bool> DeleteSubmissionAsync(int id);
    }

}
