using System.Security.Cryptography;
using TraineeManagement.api.CustomException;
using TraineeManagement.api.Repository.FileStorage;

namespace TraineeManagement.api.Services
{
    public class LocalFileStorageService : IFileStorageService
    {

        private readonly string _absoluteStoragePath;
        private readonly ILogger<LocalFileStorageService> _logger;

        public LocalFileStorageService(IConfiguration configuration, ILogger<LocalFileStorageService> logger)
        {
            string configuredPath = configuration["FileStorageSettings:StorageRootPath"] ?? "Uploads/Submissions";

            //string baseUserProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            //_absoluteStoragePath = Path.Combine(baseUserProfile,configuredPath);

            _absoluteStoragePath = Path.GetFullPath(configuredPath);

            //_absoluteStoragePath = configuration["FileStorageSettings:StorageRootPath"] ?? "Uploads/Submissions";

            if (!Directory.Exists(_absoluteStoragePath))
            {
                Directory.CreateDirectory(_absoluteStoragePath);
            }

            _logger = logger;
        }

        public async Task<string> CalculateChecksumAsync(IFormFile file)
        {
            if (file == null || file.Length == 0) return string.Empty;

            using (var stream = file.OpenReadStream())
            using (var sha256 = SHA256.Create())
            {
                byte[] hashBytes = await sha256.ComputeHashAsync(stream);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
            }
        }

        public async Task<string> SaveFileAsync(IFormFile file, string uniqueFileName)
        {
            if (!Directory.Exists(_absoluteStoragePath))
            {
                Directory.CreateDirectory(_absoluteStoragePath);
            }

            string physicalDiskWritePath = Path.Combine(_absoluteStoragePath, uniqueFileName);

            using (var fileStream = new FileStream(physicalDiskWritePath, FileMode.Create, FileAccess.Write))
            {
                await file.CopyToAsync(fileStream);
            }

            return uniqueFileName;
        }

        public void DeleteFile(string relativePath)
        {
            string fullPath = Path.Combine(_absoluteStoragePath, relativePath);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }

        public async Task<byte[]> DownloadFile(string relativePath)
        {
            string fullPath = Path.Combine(_absoluteStoragePath, relativePath);

            if (!File.Exists(fullPath))
            {
                throw new NotFoundException("The physical file could not be found on the storage disk.");
            }

            return await File.ReadAllBytesAsync(fullPath);
        }

        public Stream DownloadFileStream(string relativePath)
        {
            string fullPath = Path.Combine(_absoluteStoragePath, relativePath);

            if (!File.Exists(fullPath))
            {
                throw new NotFoundException("The physical file could not be found on the storage disk.");
            }

            // Open the file as a stream.
            // because the Controller needs to keep it open while transmitting to the client.
            return new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);
        }


        public bool isFileExists(string relativePath)
        {
            //string fullPath = Path.Combine(_absoluteStoragePath, relativePath);

            string fileName = Path.GetFileName(relativePath);

            string fullPath = Path.Combine(_absoluteStoragePath, fileName);

            // Hardcode the known absolute path that matches your Docker container exactly
            //string absoluteDockerPath = $"/App/Uploads/Submissions/{fileName}";

            _logger.LogInformation($"[DEBUG] _absoluteStoragePath: {_absoluteStoragePath}");
            _logger.LogInformation($"[DEBUG] relativePath parameter: {relativePath}");
            //_logger.LogInformation($"[DEBUG] Computed fullPath checked by .NET: {fullPath}");
            //_logger.LogInformation($"[DEBUG] Does .NET see it? {File.Exists(fullPath)}");

            return File.Exists(fullPath);
        }

        public async Task<string> CalculateChecksumAsync(string relativePath)
        {
            string fullPath = Path.Combine(_absoluteStoragePath, relativePath);

            if (!File.Exists(fullPath))
            {
                throw new NotFoundException($"Cannot calculate checksum. File not found at: {fullPath}");
            }

            using (var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true))
            using (var sha256 = SHA256.Create())
            {
                byte[] hashBytes = await sha256.ComputeHashAsync(stream);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
            }

        }
    }
}
