using System.Text.Json;
using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using CommandService.Models;
using CommandService.Utils.Enums;

namespace CommandService.EventProcessing;

public class EventProcessor : IEventProcessor
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IMapper _mapper;

    public EventProcessor(IServiceScopeFactory serviceScopeFactory, IMapper mapper)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _mapper = mapper;
    }

    public void ProcessEvent(string message)
    {
        switch (DetermineEventType(message))
        {
            case CommandSubscribeEvents.Platform_Published:
                AddPlatform(message);
                break;
            default:
                break;
        }
    }

    private async Task AddPlatform(string platformPublishedMessage)
    {
        var platformPublishedDto = JsonSerializer.Deserialize<PlatformPublishedDto>(platformPublishedMessage);
        var platform = _mapper.Map<Platform>(platformPublishedDto);
        platform.Id = 0;

        using var serviceScope = _serviceScopeFactory.CreateScope();
        var platformRepository = serviceScope.ServiceProvider.GetRequiredService<IPlatformRepository>();

        var externalPlatformExists = await platformRepository.ExternalPlatformExistsAsync(platform.ExternalId);

        if (externalPlatformExists)
        {
            Console.WriteLine("--> Platform already exists");
            return;
        }

        await platformRepository.CreatePlatformAsync(platform);
        await platformRepository.SaveChangesAsync();
        Console.WriteLine("--> Platform Created!");
    }

    private CommandSubscribeEvents DetermineEventType(string notificationMessage)
    {
        if (String.IsNullOrEmpty(notificationMessage)) return CommandSubscribeEvents.Undetermined;

        var genericEventDto = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);

        if (genericEventDto == null) return CommandSubscribeEvents.Undetermined;

        return genericEventDto.Event switch
        {
            nameof(CommandSubscribeEvents.Platform_Published) => CommandSubscribeEvents.Platform_Published,
            _ => CommandSubscribeEvents.Undetermined
        };
    }
}
