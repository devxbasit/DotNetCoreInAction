using PlatformService.Models;
using static System.Console;

namespace PlatformService.Data;

public static class PrepDb
{
    public static void PrePopulation(IApplicationBuilder app)
    {
        using IServiceScope serviceScope = app.ApplicationServices.CreateScope();
        SeedData(serviceScope.ServiceProvider.GetRequiredService<AppDbContext>());
    }

    private static void SeedData(AppDbContext dbContext)
    {
        if (!dbContext.Platforms.Any())
        {
            WriteLine("--> Seeding Data...");

            dbContext.Platforms.AddRange(
                new Platform() { Name = "Dot Net", Publisher = "Microsoft", Cost = "Free" },
                new Platform() { Name = "SQL Server Express", Publisher = "Microsoft", Cost = "Free" },
                new Platform() { Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation", Cost = "Free" }
            );

            dbContext.SaveChanges();
        }
        else
        {
            WriteLine("--> we already have platforms data");
        }
    }
}
