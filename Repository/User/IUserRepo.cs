using TraineeManagement.api.Enum;

namespace TraineeManagement.api.Repository.User
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
