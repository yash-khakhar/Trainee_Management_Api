using Microsoft.AspNetCore.Identity;
using TraineeManagement.api.Repository;

namespace TraineeManagement.api.Services
{
    public class PasswordService : IPasswordService
    {
        private readonly PasswordHasher<string> _hasher = new PasswordHasher<string>();
        public string HashPassword(string password)
        {
            return _hasher.HashPassword(null!, password);
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            var result = _hasher.VerifyHashedPassword(null!, hashedPassword, password);
            return result == PasswordVerificationResult.Success;
        }
    }
}
