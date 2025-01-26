using AutoMapper;
using CommandService.Models;
using Grpc.Net.Client;
using PlatformService;

namespace CommandService.SyncDataServices.Grpc;

public class GrpcPlatformDataClient : IPlatformDataClient
{
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public GrpcPlatformDataClient(IConfiguration configuration, IMapper mapper)
    {
        _configuration = configuration;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Platform>?> ReturnAllPlatforms()
    {
        var platformGrpcBaseUrl = _configuration.GetRequiredSection("GrpcPlatformBaseUrl").Value;
        Console.WriteLine($"--> Calling GRPC Service {platformGrpcBaseUrl}");

        var channel = GrpcChannel.ForAddress(platformGrpcBaseUrl);
        var client = new PlatformGrpcService.PlatformGrpcServiceClient(channel);
        var request = new GetAllPlatformGrpcRequest();

        try
        {
            var reply = await client.GetAllPlatformsGrpcAsync(request);
            return _mapper.Map<IEnumerable<Platform>>(reply.Platforms);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"--> Could not call GRPC Server: {ex.Message}");
            return null;
        }
    }
}
