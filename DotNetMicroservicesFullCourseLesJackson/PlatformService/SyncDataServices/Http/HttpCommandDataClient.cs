using System.Text;
using System.Text.Json;
using PlatformService.Dtos;

namespace PlatformService.SyncDataServices.Http;

public class HttpCommandDataClient(HttpClient httpClient, IConfiguration configuration) : ICommandDataClient
{
    public async Task SendPlatformToCommand(PlatformReadDto platformReadDto)
    {
        try
        {
            var httpContent = new StringContent(JsonSerializer.Serialize(platformReadDto), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(configuration["CommandService"], httpContent);


            response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> Sync Post Ok");
            }
            else
            {
                Console.WriteLine($"--> Sync Post Error: {response}");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
