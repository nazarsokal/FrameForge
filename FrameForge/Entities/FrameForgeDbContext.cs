using Microsoft.EntityFrameworkCore;

namespace Entities;

public class FrameForgeDbContext : DbContext
{
    public DbSet<Student> Students { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>().ToTable("Student");
        
        //Seed data to Students
        modelBuilder.Entity<Student>().HasData(new Student() 
            {StudentId = Guid.NewGuid(), Username = "TestUserName", Email = "TestUser@test.com", Password = "TestPassword", MoneyAmount = 100.45});
    }
}