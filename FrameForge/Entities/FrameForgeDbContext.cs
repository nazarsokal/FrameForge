using Microsoft.EntityFrameworkCore;

namespace Entities;

public class FrameForgeDbContext : DbContext
{     
    public FrameForgeDbContext(DbContextOptions options) : base(options)
    {
        
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<EnrolledLevels> LevelsEnrolled { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<EnrolledLevels>().ToTable("EnrolledLevels");
        
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

    }
}