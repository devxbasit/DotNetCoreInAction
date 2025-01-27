using CommandService.Models;
using CommandService.SyncDataServices.Grpc;

namespace CommandService.Data;

public static class PrepDb
{
    public static async Task PrePopulation(IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateAsyncScope();

        var grpcClient = serviceScope.ServiceProvider.GetRequiredService<IPlatformDataClient>();
        var platforms = await grpcClient.ReturnAllPlatforms();

        if (platforms is not null)
        {
            await SeedData(serviceScope.ServiceProvider.GetRequiredService<IPlatformRepository>(), platforms);
        }
        else
        {
            Console.WriteLine("--> GRPC null data received!");
        }
    }

    private static async Task SeedData(IPlatformRepository platformRepository, IEnumerable<Platform> platforms)
    {
        Console.WriteLine("--> Seeding new platforms...");

        foreach (var platform in platforms)
        {
            var externalPlatformExists = await platformRepository.ExternalPlatformExistsAsync(platform.ExternalId);

            if (!externalPlatformExists)
            {
                await platformRepository.CreatePlatformAsync(platform);
            }
        }

        await platformRepository.SaveChangesAsync();
    }
}
