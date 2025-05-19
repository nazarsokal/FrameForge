using Microsoft.EntityFrameworkCore;

namespace Entities;

public class FrameForgeDbContext : DbContext
{     
    public FrameForgeDbContext(DbContextOptions options) : base(options)
    {
        
    }
    
    public DbSet<Student> Students { get; set; }
    public DbSet<EnrolledLevels> LevelsEnrolled { get; set; }
    public DbSet<Test> Tests { get; set; }
    
    public DbSet<Algorithm> Algorithms { get; set; }
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>().ToTable("Student");
        modelBuilder.Entity<EnrolledLevels>().ToTable("EnrolledLevels");
        modelBuilder.Entity<Algorithm>().ToTable("Algorithm");
        
        //Seed data to Students
        modelBuilder.Entity<Student>().HasData(new Student() 
            {StudentId = Guid.NewGuid(), Username = "TestUserName", Email = "TestUser@test.com", Password = "TestPassword", MoneyAmount = 100.45});

        // Fluent API
        modelBuilder.Entity<Student>().Property(temp => temp.Email)
            .HasColumnType("varchar(40)");
        
        modelBuilder.Entity<Student>().Property(temp => temp.Password)
            .HasColumnType("nvarchar(200)");
        
        modelBuilder.Entity<Student>().Property(temp => temp.GoogleId)
            .HasColumnType("varchar(40)");
    }
}