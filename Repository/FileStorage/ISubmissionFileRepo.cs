namespace TraineeManagement.api.Repository.FileStorage
{
    public interface ISubmissionFileRepo
    {
        int Id { get; set; }
        int SubmissionId { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public long FileSizeInBytes { get; set; }
        public string ContentType { get; set; }
        public string Checksum { get; set; }
        public DateTime UploadedDate { get; set; }
    }
}
