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
        public DbSet<TaskAssignmentModel> TaskAssignment { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TaskAssignmentModel>(entity =>
            {

                entity.HasOne(d => d.Trainee)
                    .WithMany()
                    .HasForeignKey(d => d.TraineeId)
                    .OnDelete(DeleteBehavior.Cascade);

               
                entity.HasOne(d => d.Task)
                      .WithMany()
                      .HasForeignKey(d => d.TaskId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Mentor)
                      .WithMany()
                      .HasForeignKey(d => d.MentorId)
                      .OnDelete(DeleteBehavior.Cascade);

            });


        }


    }


}
