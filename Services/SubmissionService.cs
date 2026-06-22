using Microsoft.EntityFrameworkCore;
using TraineeManagement.api.CustomException;
using TraineeManagement.api.Data;
using TraineeManagement.api.DTO.SubmissionDto;
using TraineeManagement.api.Helper;
using TraineeManagement.api.Models;
using TraineeManagement.api.Redis.CacheKeys;
using TraineeManagement.api.Redis.Repository;
using TraineeManagement.api.Repository.FileStorage;
using TraineeManagement.api.Repository.Submission;

namespace TraineeManagement.api.Services
{
    public class SubmissionService : ISubmissionService
    {
        private AppDbContext _context;
        private readonly IFileStorageService _storageService;
        private readonly SubmissionFileValidator _submissionFileValidator;
        private readonly IRedisCacheRepo _redisCacheRepo;
        private readonly ILogger<SubmissionService> _logger;

        public SubmissionService(
            AppDbContext context,
            IFileStorageService storageService,
            SubmissionFileValidator submissionFileValidator,
            IRedisCacheRepo redisCacheRepo,
            ILogger<SubmissionService> logger
        )
        {
            _context = context;

            _storageService = storageService;

            _submissionFileValidator = submissionFileValidator;

            _redisCacheRepo = redisCacheRepo;

            _logger = logger;

        }

        public async Task<SubmissionResponse> AddSubmission(CreateSubmissionRequest submissionRequest, List<IFormFile> files)
        {

            DateTime dueDate = await _context.TaskAssignment
                .Where(t => t.Id == submissionRequest.TaskAssignmentId)
                .Select(t => t.DueDate).FirstOrDefaultAsync();

            if (submissionRequest.SubmittedDate > dueDate) throw new Exception("You cannot submit task after due date!");

            SubmissionModel submissionModel = new SubmissionModel(
                submissionRequest.TaskAssignmentId,
                submissionRequest.SubmissionUrl,
                submissionRequest.Notes,
                submissionRequest.SubmittedDate,
                submissionRequest.Status
            );

            var physicallyWrittenPaths = new List<string>();

            try
            {
                if (files != null)
                {
                    foreach (var file in files)
                    {
                        if (file.Length == 0) throw new Exception("Invalid File Content");
                        
                        if (_submissionFileValidator.ValidateFileSize(file.Length))
                        {
                            throw new InvalidFileSubmission("File size exceeds the 2 MB limit.");
                        }

                        if (_submissionFileValidator.ValidateFileExtension(file.FileName) == false)
                        {
                            throw new InvalidFileSubmission("Only .txt, .pdf, or .docx files are allowed.");
                        }

                        string checksum = await _storageService.CalculateChecksumAsync(file);

                        if (submissionModel.SubmissionFiles.Any(f => f.Checksum == checksum))
                        {
                            throw new InvalidFileSubmission($"You cannot upload the duplicate file '{file.FileName}' in the same submission.");
                        }

                        string uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
                        string finalRelativePath = await _storageService.SaveFileAsync(file, uniqueFileName);

                        physicallyWrittenPaths.Add(finalRelativePath);

                        var fileModel = new SubmissionFileModel
                        {
                            FileName = file.FileName,
                            FilePath = finalRelativePath,
                            FileSizeInBytes = file.Length,
                            ContentType = file.ContentType,
                            Checksum = checksum,
                            Submission = submissionModel
                        };

                        submissionModel.SubmissionFiles.Add(fileModel);
                    }
                }

                _context.Submission.Add(submissionModel);
                await _context.SaveChangesAsync();

                // remove submission data from cache
                await _redisCacheRepo.RemoveItem(SubmissionCacheKey.AllSubmissions);

                return SubmissionModel.ToDto(submissionModel);
            }
            catch (InvalidFileSubmission)
            {
                throw;
            }
            catch (Exception)
            {
                foreach (var path in physicallyWrittenPaths)
                {
                    _storageService.DeleteFile(path);
                }
                throw;
            }
        }

        public async Task<SubmissionResponse> GetSubmissionById(int id)
        {

            string cacheKey = $"{SubmissionCacheKey.SingleSubmission}:{id}";

            SubmissionResponse? cachedData = await _redisCacheRepo.GetItem<SubmissionResponse>(cacheKey);

            if (cachedData != null)
            {
                return cachedData;
            }
            else
            {
                _logger.LogInformation(
                    "Submission ID {SubmissionId} not found in Redis cache using key {RedisKey}. Falling back to MySQL DB to fetch.",
                    id,
                    cacheKey
                );


                SubmissionResponse? submissionResponse = await _context.Submission
                    .Where(s => s.Id == id)
                    .Select(s => new SubmissionResponse(
                        s.Id, 
                        s.TaskAssignmentId, 
                        s.SubmissionUrl, 
                        s.Notes, 
                        s.SubmittedDate, 
                        s.Status
                    ))
                    .FirstOrDefaultAsync();

                if (submissionResponse == null) throw new NotFoundException("Submission Not Found");

                await _redisCacheRepo.SetItem<SubmissionResponse>(cacheKey, submissionResponse);

                return submissionResponse;

            }


        }

        public async Task<IEnumerable<SubmissionResponse>> GetSubmissionList()
        {

            string cacheKey = SubmissionCacheKey.AllSubmissions;

            IEnumerable<SubmissionResponse>? cachedData = await _redisCacheRepo.GetItem<IEnumerable<SubmissionResponse>>(cacheKey);

            if (cachedData != null)
            {
                return cachedData;
            }
            else
            {
                _logger.LogInformation(
                    "Submission List not found in Redis cache using key {RedisKey}. Falling back to MySQL DB to fetch.",
                    cacheKey
                );


                List<SubmissionResponse> submissionResponse = await _context.Submission
                    .Select(s => new SubmissionResponse(
                        s.Id, 
                        s.TaskAssignmentId, 
                        s.SubmissionUrl, 
                        s.Notes, 
                        s.SubmittedDate, 
                        s.Status
                    ))
                    .ToListAsync();

                await _redisCacheRepo.SetItem<IEnumerable<SubmissionResponse>>(cacheKey, submissionResponse);

                return submissionResponse;

            }


        }

        public async Task<(byte[] FileBytes, string ContentType, string FileName)> DownloadFileAsync(int fileMetadataId)
        {
            var fileRecord = await _context.SubmissionFile
                .FirstOrDefaultAsync(f => f.Id == fileMetadataId);

            if (fileRecord == null)
            {
                _logger.LogError(
                    "Submission File Metadata ID {fileMetaId} not found in database.",
                    fileMetadataId
                );

                throw new NotFoundException("The requested file record does not exist.");
            }

            byte[] fileBytes = await _storageService.DownloadFile(fileRecord.FilePath);

            return (fileBytes, fileRecord.ContentType, fileRecord.FileName);
        }

        public async Task<bool> DeleteSubmissionAsync(int id)
        {
            SubmissionModel? submissionModel = await _context.Submission.FindAsync(id);

            if (submissionModel == null) throw new NotFoundException("Submission Not Found!. Please Try Again.");

            List<SubmissionFileModel> submissionFiles = await _context.SubmissionFile.Where(s => s.SubmissionId == id).ToListAsync();

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {

                foreach(var fileMetadata in submissionFiles)
                {

                    if (!_storageService.isFileExists(fileMetadata.FilePath)) throw new NotFoundException("Requested File does not exists on server.");

                    _storageService.DeleteFile(fileMetadata.FilePath);

                }

                _context.SubmissionFile.RemoveRange(submissionFiles);
                _context.Submission.Remove(submissionModel);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                // remove submission data from cache
                await _redisCacheRepo.RemoveItem(SubmissionCacheKey.AllSubmissions);
                await _redisCacheRepo.RemoveItem($"{SubmissionCacheKey.SingleSubmission}:{id}");

                return true;

            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
