using Microsoft.Extensions.Options;
using TraineeManagement.api.Repository.FileStorage;

namespace TraineeManagement.api.Helper
{
    public class SubmissionFileValidator
    {
        private readonly FileStorageOptions _fileStorageOptions;
        
        public SubmissionFileValidator(IOptions<FileStorageOptions> fileStorageOptions)
        {
            _fileStorageOptions = fileStorageOptions.Value;
        }

        public bool ValidateFileSize(long fileSize)
        {
            long AllowedMaxBytes = _fileStorageOptions.MaxSizeMb * 1024 * 1024;

            return fileSize > AllowedMaxBytes;

        }


        public bool ValidateFileExtension(string fileName)
        {
          
            List<string> allowedExtensions = _fileStorageOptions.AllowedExtension;
        
            var extension = Path.GetExtension(fileName).ToLowerInvariant();

            return allowedExtensions.Contains(extension);

        }

    }
}
