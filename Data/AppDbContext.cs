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
        public DbSet<SubmissionModel> Submission { get; set; }
        public DbSet<ReviewModel> Review { get; set; }
        public DbSet<SubmissionFileModel> SubmissionFile { get; set; }
        public DbSet<ProcessingJobModel> ProcessingJob { get; set; }


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

            modelBuilder.Entity<SubmissionModel>(entity =>
            {
                entity.HasOne(d => d.TaskAssignment)
                    .WithMany()
                    .HasForeignKey(d => d.TaskAssignmentId)
                    .OnDelete(DeleteBehavior.Cascade);

            });

            modelBuilder.Entity<ReviewModel>(entity =>
            {
                entity.HasOne(d => d.Submission)
                    .WithMany()
                    .HasForeignKey(d => d.SubmissionId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Mentor)
                    .WithMany()
                    .HasForeignKey(d => d.MentorId)
                    .OnDelete(DeleteBehavior.Cascade);

            });

            modelBuilder.Entity<UserModel>()
            .HasOne(u => u.Trainee)
            .WithOne(t => t.User)
            .HasForeignKey<TraineeModel>(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserModel>()
            .HasOne(u => u.Mentor)          
            .WithOne(m => m.User)           
            .HasForeignKey<MentorModel>(m => m.UserId) 
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SubmissionFileModel>()
            .HasOne(f => f.Submission)
            .WithMany(s => s.SubmissionFiles)
            .HasForeignKey(f => f.SubmissionId)
            .OnDelete(DeleteBehavior.Cascade); 


            // table properties constraints
            modelBuilder.Entity<SubmissionFileModel>()
                .Property(f => f.FilePath)
                .IsRequired()
                .HasMaxLength(500);

            modelBuilder.Entity<SubmissionFileModel>()
                .Property(f => f.FileName)
                .IsRequired()
                .HasMaxLength(255);

        }


    }


}
