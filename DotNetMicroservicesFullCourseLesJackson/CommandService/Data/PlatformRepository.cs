using CommandService.Models;
using Microsoft.EntityFrameworkCore;

namespace CommandService.Data;

public class PlatformRepository : IPlatformRepository
{
    private readonly AppDbContext _context;

    public PlatformRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> SaveChangesAsync()
    {
        var affectedRow = await _context.SaveChangesAsync();
        return affectedRow >= 0;
    }

    public async Task<IEnumerable<Platform>> GetAllPlatformsAsync()
    {
        var platforms = await _context.Platforms.AsNoTracking().ToListAsync();
        return platforms;
    }

    public async Task CreatePlatformAsync(Platform platform)
    {
        ArgumentNullException.ThrowIfNull(platform);
        await _context.Platforms.AddAsync(platform);
    }

    public async Task<bool> PlatformExitsAsync(int platformId)
    {
        var platformExists = await _context.Platforms.AnyAsync(x => x.Id == platformId);
        return platformExists;

    }

    public async Task<bool> ExternalPlatformExistsAsync(int externalPlatformId)
    {
        var externalPlatformExists = await _context.Platforms.AnyAsync(x => x.ExternalId == externalPlatformId);
        return externalPlatformExists;
    }
}
