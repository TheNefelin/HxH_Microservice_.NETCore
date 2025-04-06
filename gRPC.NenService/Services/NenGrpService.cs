using gRPC.NenService.Protos;

namespace gRPC.NenService.Services;

public class NenGrpService : NenServiceProto.NenServiceProtoBase
{
    private readonly ILogger<NenGrpService> _logger;

    public NenGrpService(ILogger<NenGrpService> logger)
    {
        _logger = logger;
    }
}
