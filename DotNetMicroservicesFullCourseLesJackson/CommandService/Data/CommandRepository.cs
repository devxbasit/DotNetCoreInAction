using CommandService.Models;
using Microsoft.EntityFrameworkCore;

namespace CommandService.Data;

public class CommandRepository : ICommandRepository
{
    private readonly AppDbContext _context;

    public CommandRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> SaveChangesAsync()
    {
        var affectedRows = await _context.SaveChangesAsync();
        return affectedRows >= 0;
    }

    public async Task CreateCommandAsync(int platformId, Command command)
    {
        ArgumentNullException.ThrowIfNull(command);

        command.PlatformId = platformId;
        await _context.Commands.AddAsync(command);
    }

    public async Task<Command?> GetCommandAsync(int platformId, int commandId)
    {
        var command = await _context.Commands.FirstOrDefaultAsync(x => x.PlatformId == platformId && x.Id == commandId);
        return command;
    }

    public async Task<IEnumerable<Command>> GetCommandsForPlatformAsync(int platformId)
    {
        var commands = _context.Commands.Where(x => x.PlatformId == platformId).AsNoTracking().OrderBy(x => x.Platform.Name);
        await Task.CompletedTask;
        return commands;
    }
}
