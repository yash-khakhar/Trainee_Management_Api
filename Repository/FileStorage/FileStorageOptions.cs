namespace TraineeManagement.api.Repository.FileStorage
{
    public class FileStorageOptions
    {
        public int MaxSizeMb { get; set; }
        public List<string> AllowedExtension { get; set; } = new List<string>();
    }
}
