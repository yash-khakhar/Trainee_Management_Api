using TraineeManagement.api.Enum.Trainee;
using TraineeManagement.api.Enum.User;

namespace TraineeManagement.api.Repository
{
    public interface IUserRepo
    {
        int Id { get; set; }
        string UserName { get; set; }
        string Email { get; set; }
        string PasswordHash { get; set; }
        UserRolesEnum Role { get; set; }
        DateTime CreatedAt { get; set; }
        DateTime UpdatedAt { get; set; }
    }
}
