using ClassLibrary.Core.DTOs;
using ClassLibrary.Core.Interfaces;
using gRPC.HunterService.Protos;
using Grpc.Core;

namespace gRPC.HunterService.Services;

public class HunterGrpcService : HunterServiceProto.HunterServiceProtoBase
{
    private readonly ILogger<HunterGrpcService> _logger;
    private readonly IHunterService _hunterService;

    public HunterGrpcService(ILogger<HunterGrpcService> logger, IConfiguration configuration, IHunterService hunterService)
    {
        _logger = logger;
        _hunterService = hunterService;
    }

    public override async Task<HunterListResponse> GetAllHunter(Empty request, ServerCallContext context)
    {
        _logger.LogInformation("gRPC request: GetAllHunter");
        var hunters = await _hunterService.GetAllAsync();
        var response = new HunterListResponse();

        foreach (var hunter in hunters)
        {
            response.Hunters.Add(new HunterResponse
            {
                Id = hunter.Id_Hunter,
                Name = hunter.Name,
                Age = hunter.Age,
                Origin = hunter.Origin
            });
        }

        _logger.LogInformation("Retrieved {Count} hunters", response.Hunters.Count);
        return response;
    }

    public override async Task<HunterResponse> GetHunterById(HunterIdRequest request, ServerCallContext context)
    {
        _logger.LogInformation("gRPC request: GetHunterById - ID: {Id}", request.Id);
        var hunter = await _hunterService.GetByIdAsync(request.Id);
        
        if (hunter == null)
        {
            _logger.LogWarning("Hunter not found with ID: {Id}", request.Id);
            throw new RpcException(new Status(StatusCode.NotFound, "Hunter not found"));
        }

        _logger.LogInformation("Hunter found: {Name}", hunter.Name);
        return new HunterResponse
        {
            Id = hunter.Id_Hunter,
            Name = hunter.Name,
            Age = hunter.Age,
            Origin = hunter.Origin
        };
    }

    public override async Task<GenericResponse> CreateHunter(HunterRequest request, ServerCallContext context)
    {
        _logger.LogInformation("gRPC request: CreateHunter - Name: {Name}", request.Name);

        var dto = new HunterDTO
        {
            Name = request.Name,
            Age = request.Age,
            Origin = request.Origin
        };

        var success = await _hunterService.CreateAsync(dto);

        if (success)
            _logger.LogInformation("Hunter created successfully: {Name}", request.Name);
        else
            _logger.LogWarning("Failed to create hunter: {Name}", request.Name);

        return new GenericResponse
        {
            Success = success,
            Message = success ? "Hunter created successfully" : "Failed to create hunter"
        };
    }

    public override async Task<GenericResponse> UpdateHunter(HunterUpdateRequest request, ServerCallContext context)
    {
        _logger.LogInformation("gRPC request: UpdateHunter - ID: {Id}", request.Id);

        var dto = new HunterDTO
        {
            Id_Hunter = request.Id,
            Name = request.Name,
            Age = request.Age,
            Origin = request.Origin
        };

        var success = await _hunterService.UpdateAsync(dto);

        if (success)
            _logger.LogInformation("Hunter updated successfully: {Id}", request.Id);
        else
            _logger.LogWarning("Failed to update hunter: {Id}", request.Id);

        return new GenericResponse
        {
            Success = success,
            Message = success ? "Hunter updated successfully" : "Failed to update hunter"
        };
    }

    public override async Task<GenericResponse> DeleteHunter(HunterIdRequest request, ServerCallContext context)
    {
        _logger.LogInformation("gRPC request: DeleteHunter - ID: {Id}", request.Id);

        var success = await _hunterService.DeleteAsync(request.Id);

        if (success)
            _logger.LogInformation("Hunter deleted successfully: {Id}", request.Id);
        else
            _logger.LogWarning("Failed to delete hunter or not found: {Id}", request.Id);

        return new GenericResponse
        {
            Success = success,
            Message = success ? "Hunter deleted successfully" : "Hunter not found or failed to delete"
        };
    }
}

