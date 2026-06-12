using Microsoft.AspNetCore.Identity;
using TraineeManagement.api.Data;
using TraineeManagement.api.Models;
using TraineeManagement.api.Repository.Password;

namespace TraineeManagement.api.Services
{
    public class PasswordService : IPasswordService
    {
        private readonly PasswordHasher<string> _hasher = new PasswordHasher<string>();
        private AppDbContext _context;
        
        public PasswordService(AppDbContext context)
        {
            _context = context;
        }
        public string HashPassword(string password)
        {
            return _hasher.HashPassword(null!, password);
        }

        public async Task<bool> VerifyPassword(string password, UserModel user)
        {
            
            var result = _hasher.VerifyHashedPassword(null!, user.PasswordHash, password);

            switch (result)
            {
                case PasswordVerificationResult.Success: return true;

                case PasswordVerificationResult.SuccessRehashNeeded:
                    user.PasswordHash = HashPassword(password);
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                    return true;

                case PasswordVerificationResult.Failed: return false;

                default: return false;
            }
        }
    }
}
