using CommandService.Models;

namespace CommandService.Data;

public interface IPlatformRepository
{
    Task<bool> SaveChangesAsync();
    Task<IEnumerable<Platform>> GetAllPlatformsAsync();
    Task CreatePlatformAsync(Platform platform);
    Task<bool> PlatformExitsAsync(int platformId);
    Task<bool> ExternalPlatformExistsAsync(int externalPlatformId);
}
