using System.Security.Cryptography;
using TraineeManagement.api.CustomException;
using TraineeManagement.api.Repository.FileStorage;

namespace TraineeManagement.api.Services
{
    public class LocalFileStorageService : IFileStorageService
    {

        private readonly string _absoluteStoragePath;

        public LocalFileStorageService(IConfiguration configuration)
        {
            string configuredPath = configuration["FileStorageSettings:StorageRootPath"] ?? "Uploads/Submissions";

            string baseUserProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            _absoluteStoragePath = Path.Combine(baseUserProfile, configuredPath);

            if (!Directory.Exists(_absoluteStoragePath))
            {
                Directory.CreateDirectory(_absoluteStoragePath);
            }
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
            string fullPath = Path.Combine(_absoluteStoragePath, relativePath);
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
