using ClassLibrary.Core.Interfaces;
using gRPC.HunterNenService.Mappers;
using Grpc.Core;

namespace gRPC.HunterNenService.Services;

public class GrpcHunterNenService : HunterNenProto.HunterNenProtoBase
{
    private readonly ILogger<GrpcHunterNenService> _logger;
    private readonly IHunterNenService _hunterNenService;

    public GrpcHunterNenService(ILogger<GrpcHunterNenService> logger, IHunterNenService hunterNenService)
    {
        _logger = logger;
        _hunterNenService = hunterNenService;
    }

    public override async Task<HunterNenListResponse> HunterNenGetAll(Empty request, ServerCallContext context)
    {
        _logger.LogInformation("gRPC request: HunterNenGetAll");
        var hunterNens = await _hunterNenService.GetAllHunterNensAsync();
        var response = new HunterNenListResponse();

        foreach (var hunterNen in hunterNens)
        {
            response.HunterNens.Add(new HunterNenResponse
            {
                IdHunter = hunterNen.Id_Hunter,
                IdNen = hunterNen.Id_NenType,
                NenLevel = hunterNen.NenLevel
            });
        }

        _logger.LogInformation("Retrieved {Count} HunterNens", response.HunterNens.Count);
        return response;
    }

    public override async Task<HunterNenResponse> HunterNenGetById(HunterNenRequest request, ServerCallContext context)
    {
        _logger.LogInformation("gRPC request: HunterNenGetById for Hunter {IdHunter} and Nen {IdNen}", request.IdHunter, request.IdNen);
        var dto = HunterNenMapper.ToDTO(request);
        var result = await _hunterNenService.GetHunterNenByIdAsync(dto);

        if (result == null)
        {
            _logger.LogWarning("HunterNen not found for Hunter {IdHunter}, Nen {IdNen}", request.IdHunter, request.IdNen);
            throw new RpcException(new Status(StatusCode.NotFound, "HunterNen not found"));
        }

        _logger.LogInformation("HunterNen found: Hunter {IdHunter}, Nen {IdNen}", result.Id_Hunter, result.Id_NenType);
        return new HunterNenResponse
        {
            IdHunter = result.Id_Hunter,
            IdNen = result.Id_NenType,
            NenLevel = result.NenLevel
        };
    }

    public override async Task<GenericResponse> HunterNenInsert(HunterNenRequest request, ServerCallContext context)
    {
        _logger.LogInformation("gRPC request: HunterNenInsert for Hunter {IdHunter}, Nen {IdNen}", request.IdHunter, request.IdNen);

        var dto = HunterNenMapper.ToDTO(request);
        var result = await _hunterNenService.InsertHunterNenAsync(dto);

        if (!result.IsSuccess)
        {
            _logger.LogWarning("Insert failed: {Message}", result.Message);
            return new GenericResponse { Success = false, Message = result.Message };
        }

        _logger.LogInformation("Insert successful for Hunter {IdHunter}, Nen {IdNen}", request.IdHunter, request.IdNen);
        return new GenericResponse { Success = true, Message = "HunterNen inserted successfully" };
    }


    public override async Task<GenericResponse> HunterNenUpdate(HunterNenRequest request, ServerCallContext context)
    {
        _logger.LogInformation("gRPC request: HunterNenUpdate for Hunter {IdHunter}, Nen {IdNen}", request.IdHunter, request.IdNen);
        var dto = HunterNenMapper.ToDTO(request);
        var result = await _hunterNenService.UpdateHunterNenAsync(dto);

        if (!result.IsSuccess)
        {
            _logger.LogWarning("Update failed: {Message}", result.Message);
            return new GenericResponse { Success = false, Message = result.Message };
        }

        _logger.LogInformation("Update successful for Hunter {IdHunter}, Nen {IdNen}", request.IdHunter, request.IdNen);
        return new GenericResponse { Success = true, Message = "HunterNen updated successfully" };
    }

    public override async Task<GenericResponse> HunterNenDelete(HunterNenRequest request, ServerCallContext context)
    {
        _logger.LogInformation("gRPC request: HunterNenDelete for Hunter {IdHunter}, Nen {IdNen}", request.IdHunter, request.IdNen);
        var dto = HunterNenMapper.ToDTO(request);
        var result = await _hunterNenService.DeleteHunterNenAsync(dto);

        if (!result.IsSuccess)
        {
            _logger.LogWarning("Delete failed: {Message}", result.Message);
            return new GenericResponse { Success = false, Message = result.Message };
        }

        _logger.LogInformation("Delete successful for Hunter {IdHunter}, Nen {IdNen}", request.IdHunter, request.IdNen);
        return new GenericResponse { Success = true, Message = "HunterNen deleted successfully" };
    }
}
