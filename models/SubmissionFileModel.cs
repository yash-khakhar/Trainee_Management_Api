using System.ComponentModel.DataAnnotations;
using TraineeManagement.api.Repository.FileStorage;

namespace TraineeManagement.api.Models
{
    public class SubmissionFileModel : ISubmissionFileRepo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SubmissionId { get; set; }
        public SubmissionModel Submission { get; set; } = new SubmissionModel();

        [Required]
        public string FilePath { get; set; } = string.Empty;

        [Required]
        public string FileName { get; set; } = string.Empty;

        [Required]
        public long FileSizeInBytes { get; set; }

        [Required]
        public string ContentType { get; set; } = string.Empty;
        public DateTime UploadedDate { get; set; } = DateTime.UtcNow;
        
        [Required] 
        public string Checksum { get; set; } = string.Empty;
    }
}
