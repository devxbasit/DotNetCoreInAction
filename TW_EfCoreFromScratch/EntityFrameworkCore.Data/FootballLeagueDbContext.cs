using EntityFrameworkCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EntityFrameworkCore.Data;

public class FootballLeagueDbContext : DbContext
{
    public DbSet<League> Leagues { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<Match> Matches { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = "Data Source = localhost,1433; Initial Catalog = EfCoreDb_Trevoir; Integrated Security = false; User Id = sa; Password = docker-147852369; TrustServerCertificate = true";

        optionsBuilder.UseSqlServer(connectionString)
            .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information)
            .EnableDetailedErrors()
            .EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Team>()
            .HasMany(x => x.HomeMatches)
            .WithOne(x => x.HomeTeam)
            .HasForeignKey(x => x.HomeTeamId)
            .IsRequired();

        modelBuilder.Entity<Team>()
            .HasMany(x => x.AwayMatches)
            .WithOne(x => x.AwayTeam)
            .HasForeignKey(x => x.AwayTeamId)
            .IsRequired();
    }
}
