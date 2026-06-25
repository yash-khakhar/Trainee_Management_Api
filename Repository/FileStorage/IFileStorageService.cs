namespace TraineeManagement.api.Repository.FileStorage
{
    public interface IFileStorageService
    {
        Task<string> CalculateChecksumAsync(IFormFile file);
        Task<string> CalculateChecksumAsync(string relativePath);
        Task<string> SaveFileAsync(IFormFile file, string uniqueFileName);
        void DeleteFile(string relativePath);
        Task<byte[]> DownloadFile(string relativePath);
        Stream DownloadFileStream(string relativePath);
        bool isFileExists(string relativePath);
    }
}
