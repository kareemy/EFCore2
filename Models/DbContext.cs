using Microsoft.EntityFrameworkCore;

namespace EFCore2.Models;

public class AppDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=database.db");
    }

    // Needed for Many-to-Many association entity
    // StudentCourse entity has 2 attributes as the primary key.
    // This code tells EF Core that StudentID and CourseID combine for the primary key
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StudentCourse>()
            .HasKey(e => new {e.StudentID, e.CourseID});
    }

    public DbSet<Course> Courses {get; set;}
    public DbSet<Professor> Professors {get; set;}
    public DbSet<Student> Students {get; set;}
    public DbSet<StudentCourse> StudentCourses {get; set;}
}