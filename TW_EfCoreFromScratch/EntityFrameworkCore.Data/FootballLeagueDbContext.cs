using EntityFrameworkCore.Data.Configurations.Entities;
using EntityFrameworkCore.Domain;
using EntityFrameworkCore.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EntityFrameworkCore.Data;

public class FootballLeagueDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = "Data Source = localhost,1433; Initial Catalog = EfCoreDb_Trevoir; Integrated Security = false; User Id = sa; Password = docker-147852369; TrustServerCertificate = true";

        optionsBuilder.UseSqlServer(connectionString, sqlServerOptionsAction: sqlOptions => { sqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null); })
            .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information)
            .EnableDetailedErrors()
            .EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly();
        modelBuilder.ApplyConfiguration(new LeagueConfiguration());
        modelBuilder.ApplyConfiguration(new TeamConfiguration());
        modelBuilder.ApplyConfiguration(new CoachConfiguration());

        var foreignKeys = modelBuilder
            .Model
            .GetEntityTypes()
            .SelectMany(x => x.GetForeignKeys())
            .Where(x => x.DeleteBehavior == DeleteBehavior.Cascade && x.IsOwnership == false);

        foreach (var foreignKey in foreignKeys)
        {
            foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        // Set max length for all string properties to 50
        configurationBuilder.Properties<string>().HaveMaxLength(50);
    }


    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries()
            .Where(x => x.State == EntityState.Added || x.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            var auditableObject = (BaseDomainEntity)entry.Entity;
            auditableObject.LastUpdatedDateTime = DateTime.UtcNow;

            if (entry.State == EntityState.Added)
            {
                auditableObject.CreatedDateTime = DateTime.UtcNow;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    public DbSet<League> Leagues { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<Match> Matches { get; set; }
    public DbSet<Coach> Coaches { get; set; }
}
