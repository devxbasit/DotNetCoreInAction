using EntityFrameworkCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EntityFrameworkCore.Data;

public class FootballLeagueDbContext : DbContext
{
    public DbSet<League> Leagues { get; set; }
    public DbSet<Team> Teams { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = "Data Source = localhost,1433; Initial Catalog = EfCoreDb; Integrated Security = false; User Id = sa; Password = docker-147852369; TrustServerCertificate = true";

        optionsBuilder.UseSqlServer(connectionString)
            .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information)
            .EnableDetailedErrors()
            .EnableSensitiveDataLogging();
    }
}
