using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using CommandService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controller;

[ApiController]
[Route("/api/c/platform/{platformId}/command")]
public class CommandController : ControllerBase
{
    private readonly ICommandRepository _commandRepository;
    private readonly IPlatformRepository _platformRepository;
    private readonly IMapper _mapper;

    public CommandController(ICommandRepository commandRepository, IPlatformRepository platformRepository, IMapper mapper)
    {
        _commandRepository = commandRepository;
        _platformRepository = platformRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CommandReadDto>>> GetCommandsForPlatforms(int platformId)
    {
        var platformExits = await _platformRepository.PlatformExitsAsync(platformId);

        if (!platformExits)
        {
            return NotFound();
        }

        var commands = await _commandRepository.GetCommandsForPlatformAsync(platformId);

        return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commands));
    }

    [HttpGet("{commandId:int}")]
    public async Task<ActionResult<CommandReadDto>> GetCommandForPlatform(int platformId, int commandId)
    {
        var platformExits = await _platformRepository.PlatformExitsAsync(platformId);

        if (!platformExits)
        {
            return NotFound();
        }

        var command = await _commandRepository.GetCommandAsync(platformId, commandId);

        if (command is null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<CommandReadDto>(command));
    }

    [HttpPost]
    public async Task<ActionResult<CommandCreateDto>> CreateCommandForPlatform(int platformId, CommandCreateDto commandCreateDto)
    {
        var platformExits = await _platformRepository.PlatformExitsAsync(platformId);

        if (!platformExits)
        {
            return NotFound();
        }

        var command = _mapper.Map<Command>(commandCreateDto);

        await _commandRepository.CreateCommandAsync(platformId, command);
        await _commandRepository.SaveChangesAsync();

        var commandReadDto = _mapper.Map<CommandReadDto>(command);

        return CreatedAtAction(nameof(GetCommandForPlatform), new { platformId = platformId, commandId = commandReadDto.Id }, commandReadDto);
    }
}
