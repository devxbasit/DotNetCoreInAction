using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.AsyncDataServices.Amqp;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;
using PlatformService.Utils.Enums;
using PlatformService.Utils.StaticClasses;

namespace PlatformService.Controllers;

[ApiController]
[Route("api/platform")]
public class PlatformController : ControllerBase
{
    private readonly IPlatformRepository _platformRepository;
    private readonly IMapper _mapper;
    private readonly ICommandDataClient _commandDataClient;
    private readonly IMessageBusClient _messageBusClient;

    public PlatformController(IPlatformRepository platformRepository,
        IMapper mapper,
        ICommandDataClient commandDataClient,
        IMessageBusClient messageBusClient)
    {
        _platformRepository = platformRepository;
        _mapper = mapper;
        _commandDataClient = commandDataClient;
        _messageBusClient = messageBusClient;
    }

    [HttpGet]
    public ActionResult<IEnumerable<PlatformReadDto>> GetAllPlatforms()
    {
        var platforms = _mapper.Map<IEnumerable<PlatformReadDto>>(_platformRepository.GetAllPlatforms());
        return Ok(platforms);
    }

    [HttpGet("{id:int}")]
    public ActionResult<PlatformReadDto> GetPlatformById(int id)
    {
        var existingPlatform = _platformRepository.GetPlatformById(id);

        if (existingPlatform is null)
        {
            return BadRequest();
        }

        return Ok(_mapper.Map<PlatformReadDto>(existingPlatform));
    }

    [HttpPost]
    public async Task<ActionResult<PlatformReadDto>> CreatePlatform([FromBody] PlatformCreateDto platformCreateDto)
    {
        var platformModel = _mapper.Map<Platform>(platformCreateDto);
        _platformRepository.CreatePlatform(platformModel);
        _platformRepository.SaveChanges();

        var platformReadDto = _mapper.Map<PlatformReadDto>(platformModel);

        // send sync message to target destination(CommandServiceEndpoint)
        try
        {
            //await _commandDataClient.SendPlatformToCommand(platformReadDto);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"--> Could not send sync message to CommandService: {ex.Message}");
        }

        // send async message to message bus
        try
        {
            var platformPublishedDto = _mapper.Map<PlatformPublishedDto>(platformReadDto);
            platformPublishedDto.Event = nameof(PlatformPublishEvents.Platform_Published);
            await _messageBusClient.PublishNewPlatform(platformPublishedDto);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"--> Could not send async message: {ex.Message}");
            throw;
        }

        return CreatedAtAction(nameof(GetPlatformById), new { id = platformReadDto.Id }, platformReadDto);
    }
}
