using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Repository;

namespace LoggingWebApi.ContextFactory;


public class RepositoryContextFactory : IDesignTimeDbContextFactory<RepositoryContext>
{
    public RepositoryContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var assembly = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
        
        var builder = new DbContextOptionsBuilder<RepositoryContext>()
            .UseSqlServer(configuration.GetConnectionString("LoggingDbConnection"),
                b=> b.MigrationsAssembly("LoggingWebApi"));

        return new RepositoryContext(builder.Options);

    }
}