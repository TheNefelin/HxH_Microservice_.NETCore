using ClassLibrary.Core.Interfaces;
using gRPC.NenService.Protos;
using Grpc.Core;

namespace gRPC.NenService.Services;

public class NenGrpService : NenServiceProto.NenServiceProtoBase
{
    private readonly ILogger<NenGrpService> _logger;
    private readonly INenService _NenService;

    public NenGrpService(ILogger<NenGrpService> logger, INenService nenService)
    {
        _logger = logger;
        _NenService = nenService;
    }

    public override async Task<NenTypeListResponse> GetAllNenTypes(Empty request, ServerCallContext context)
    {
        _logger.LogInformation("gRPC request: NenGetAll");
        var nens = await _NenService.GetAllAsync();
        var response = new NenTypeListResponse();
        
        foreach (var nen in nens)
        {
            response.NenTypes.Add(new NenTypeResponse
            {
                Id = nen.Id_NenType,
                Name = nen.Name,
                Description = nen.Description
            });
        }

        _logger.LogInformation("Retrieved {Count} Nens", response.NenTypes.Count);
        return response;
    }
}
