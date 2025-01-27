using AutoMapper;
using Grpc.Core;
using PlatformService.Data;
using PlatformService.Models;

namespace PlatformService.SyncDataServices.Grpc;

public class GrpcPlatformService : PlatformGrpcService.PlatformGrpcServiceBase
{
    private readonly IPlatformRepository _platformRepository;
    private readonly IMapper _mapper;

    public GrpcPlatformService(IPlatformRepository platformRepository, IMapper mapper)
    {
        _platformRepository = platformRepository;
        _mapper = mapper;
         Console.WriteLine("--> Grpc Server ready.");
    }

    public override  Task<GetAllPlatformGrpcResponse> GetAllPlatformsGrpc(GetAllPlatformGrpcRequest request, ServerCallContext context)
    {
        Console.WriteLine($"--> Platform Grpc server: {nameof(GetAllPlatformsGrpc)} invoked!");
        var response = new GetAllPlatformGrpcResponse();
        var platforms = _platformRepository.GetAllPlatforms();

        response.Platforms.AddRange(_mapper.Map<IEnumerable<PlatformGrpcModel>>(platforms));

        Console.WriteLine("--> Platform Grpc server returning response");
        return Task.FromResult(response);
    }
}
