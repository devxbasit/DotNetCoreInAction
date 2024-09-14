using EFCoreBasic.Models;
using Microsoft.EntityFrameworkCore;
namespace EFCoreBasic.Data;

public class AppDbContext : DbContext
{

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }
    public DbSet<Student> Students { get; set; }
    public DbSet<Passport> Passports { get; set; }

    // public DbSet<Course> Courses { get; set; }
    // public DbSet<StudentClub> StudentClubs { get; set; }
}
