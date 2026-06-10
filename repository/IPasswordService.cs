namespace TraineeManagement.api.Repository
{
    public interface IPasswordService
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string hashedPassword);

    }
}
