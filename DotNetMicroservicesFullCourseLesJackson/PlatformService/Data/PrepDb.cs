using Microsoft.EntityFrameworkCore;
using PlatformService.Models;
using static System.Console;

namespace PlatformService.Data;

public static class PrepDb
{
    public static void PrePopulation(IApplicationBuilder app, IWebHostEnvironment env)
    {
        using IServiceScope serviceScope = app.ApplicationServices.CreateScope();
        SeedData(serviceScope.ServiceProvider.GetRequiredService<AppDbContext>(), env);
    }

    private static void SeedData(AppDbContext dbContext, IWebHostEnvironment env)
    {
        if (env.IsProduction() && dbContext.Database.GetMigrations().Any())
        {
            try
            {
                Console.WriteLine("Attempting to run migrations...");
                dbContext.Database.Migrate();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not run migrations: {ex.Message}");
                throw;
            }
        }


        if (!dbContext.Platforms.Any())
        {
            WriteLine("--> Seeding Data...");

            dbContext.Platforms.AddRange(
                new Platform() { Name = "Dot Net", Publisher = "Microsoft", Cost = "Free" },
                new Platform() { Name = "SQL Server Express", Publisher = "Microsoft", Cost = "Free" },
                new Platform() { Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation", Cost = "Free" }
            );

            dbContext.SaveChanges();
            dbContext.Database.Migrate();
        }
        else
        {
            WriteLine("--> Platforms data is already present, skipping data seeding!");
        }
    }
}
