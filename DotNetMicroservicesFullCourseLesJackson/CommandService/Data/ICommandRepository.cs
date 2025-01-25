using CommandService.Models;

namespace CommandService.Data;

public interface ICommandRepository
{
    Task<bool> SaveChangesAsync();
    Task CreateCommandAsync(int platformId, Command command);
    Task<Command?> GetCommandAsync(int platformId, int commandId);
    Task<IEnumerable<Command>> GetCommandsForPlatformAsync(int platformId);
}
