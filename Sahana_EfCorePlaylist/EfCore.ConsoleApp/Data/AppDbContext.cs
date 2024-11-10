using EfCore.ConsoleApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

namespace EfCore.ConsoleApp.Data;

public class AppDbContext : DbContext
{

    public DbSet<Employee> Employees { get; set; }
    public DbSet<Manager> Managers { get; set; }
    public DbSet<Project> Projects { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = "Data Source = localhost,1433; Initial Catalog = EfCoreDb_Sahana; Integrated Security = false; User Id = sa; Password = docker-147852369; TrustServerCertificate = true";

        // lazy loading with proxies - only virtual marked navigation properties will be loaded lazy
        // optionsBuilder.UseLazyLoadingProxies().UseSqlServer(connectionString);

        optionsBuilder.UseSqlServer(connectionString)
            .LogTo(Console.WriteLine, new []{ DbLoggerCategory.Database.Command.Name}, LogLevel.Information)
            .EnableDetailedErrors()
            .EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<EmployeeProject>()
            .HasKey(x => new { x.EmployeeId, x.ProjectId });

        modelBuilder.Entity<EmployeeProject>()
            .HasOne(x => x.Employee)
            .WithMany(x => x.EmployeeProjects)
            .HasForeignKey(x => x.EmployeeId);

        modelBuilder.Entity<EmployeeProject>()
            .HasOne(x => x.Project)
            .WithMany(x => x.EmployeeProjects)
            .HasForeignKey(x => x.ProjectId);

    }
}
