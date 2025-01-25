using PlatformService.Dtos;

namespace PlatformService.AsyncDataServices.Amqp;

public interface IMessageBusClient
{
    Task PublishNewPlatform(PlatformPublishedDto platformPublishedDto);
}
