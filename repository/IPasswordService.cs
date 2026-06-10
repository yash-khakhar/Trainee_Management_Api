
using TraineeManagement.api.Models;

namespace TraineeManagement.api.Repository
{
    public interface IPasswordService
    {
        string HashPassword(string password);
        Task<bool> VerifyPassword(string password, UserModel user);

    }
}
