using TraineeManagement.api.Enum;

namespace TraineeManagement.api.Repository.Task
{
    public interface ITaskRepo
    {
        int Id { get; set; }
        string Title { get; set; }
        string Description { get; set; }
        string ExpectedTechStack { get; set; }
        DateTime DueDate { get; set; }
        TaskStatusEnum Status { get; set; }
        DateTime CreatedAt { get; set; }
        DateTime UpdatedAt { get; set; }
    }
}
