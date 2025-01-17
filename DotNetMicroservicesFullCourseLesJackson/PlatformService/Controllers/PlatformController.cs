using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers;

[ApiController]
[Route("api/platform")]
public class PlatformController(
    IPlatformRepository platformRepository,
    IMapper mapper,
    ICommandDataClient commandDataClient) : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<PlatformReadDto>> GetAllPlatforms()
    {
        var platforms = mapper.Map<IEnumerable<PlatformReadDto>>(platformRepository.GetAllPlatforms());
        return Ok(platforms);
    }

    [HttpGet("{id:int}")]
    public ActionResult<PlatformReadDto> GetPlatformById(int id)
    {
        var existingPlatform = platformRepository.GetPlatformById(id);

        if (existingPlatform is null)
        {
            return BadRequest();
        }

        return Ok(mapper.Map<PlatformReadDto>(existingPlatform));
    }

    [HttpPost]
    public async Task<ActionResult<PlatformReadDto>> CreatePlatform([FromBody] PlatformCreateDto platformCreateDto)
    {
        var platformModel = mapper.Map<Platform>(platformCreateDto);
        platformRepository.CreatePlatform(platformModel);
        platformRepository.SaveChanges();

        var platformReadDto = mapper.Map<PlatformReadDto>(platformModel);

        try
        {
            await commandDataClient.SendPlatformToCommand(platformReadDto);
        }
        catch(Exception ex)
        {
            Console.WriteLine($"--> Could not send sync message to CommandService: {ex.Message}");
        }

        return CreatedAtAction(nameof(GetPlatformById), new {id = platformReadDto.Id}, platformReadDto);
    }
}
