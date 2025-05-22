using Microsoft.EntityFrameworkCore;

namespace Entities;

public class FrameForgeDbContext : DbContext
{     
    public FrameForgeDbContext(DbContextOptions options) : base(options)
    {
        
    }
    
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<EnrolledLevels> LevelsEnrolled { get; set; }
    public virtual DbSet<Group> Groups { get; set; }
    public virtual DbSet<Exercise> Exercises { get; set; }
    public virtual DbSet<ExerciseSubmission> ExerciseSubmissions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<EnrolledLevels>().ToTable("EnrolledLevels");
        modelBuilder.Entity<Group>().ToTable("Groups");
        modelBuilder.Entity<Exercise>().ToTable("Exercises");
        modelBuilder.Entity<ExerciseSubmission>().ToTable("ExerciseSubmissions");
        
        modelBuilder.Entity<User>()
            .HasDiscriminator<string>("TypeOfUser")
            .HasValue<Student>("Student")
            .HasValue<Teacher>("Teacher");
        
        modelBuilder.Entity<Group>()
            .HasOne(g => g.Teacher)
            .WithMany(t => t.Groups)
            .HasForeignKey(g => g.TeacherId);

        modelBuilder.Entity<Student>()
            .HasOne(s => s.Group)
            .WithMany(g => g.Students)
            .HasForeignKey(s => s.GroupId);

        modelBuilder.Entity<ExerciseSubmission>()
            .HasOne(s => s.StudentSubmitted);

    }
}