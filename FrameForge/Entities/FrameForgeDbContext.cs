using Microsoft.EntityFrameworkCore;

namespace Entities;

public class FrameForgeDbContext : DbContext
{     
    public FrameForgeDbContext(DbContextOptions options) : base(options)
    {
        
    }
    
    public DbSet<Student> Students { get; set; }
    public DbSet<EnrolledLevels> LevelsEnrolled { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>().ToTable("Student");
        modelBuilder.Entity<EnrolledLevels>().ToTable("EnrolledLevels");
        
        //Seed data to Students
        modelBuilder.Entity<Student>().HasData(new Student() 
            {StudentId = Guid.NewGuid(), Username = "TestUserName", Email = "TestUser@test.com", Password = "TestPassword", MoneyAmount = 100.45});
    }
}