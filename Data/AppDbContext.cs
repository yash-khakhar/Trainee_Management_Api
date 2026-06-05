using Microsoft.EntityFrameworkCore;
using TraineeManagement.api.models;

namespace TraineeManagement.api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<TraineeModel> Trainees { get; set; }
    }
}
