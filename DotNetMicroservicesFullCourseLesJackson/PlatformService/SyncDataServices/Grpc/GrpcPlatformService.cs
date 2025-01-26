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
    }

    public override  Task<GetAllPlatformGrpcResponse> GetAllPlatformsGrpc(GetAllPlatformGrpcRequest request, ServerCallContext context)
    {
        var response = new GetAllPlatformGrpcResponse();
        var platforms = _platformRepository.GetAllPlatforms();

        response.Platforms.AddRange(_mapper.Map<IEnumerable<PlatformGrpcModel>>(platforms));

        return Task.FromResult(response);
    }
}
