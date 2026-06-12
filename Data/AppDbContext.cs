using Microsoft.EntityFrameworkCore;
using TraineeManagement.api.models;
using TraineeManagement.api.Models;

namespace TraineeManagement.api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<TraineeModel> Trainees { get; set; }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<MentorModel> Mentor { get; set; }
        public DbSet<TaskModel> Task { get; set; }
    }
}
