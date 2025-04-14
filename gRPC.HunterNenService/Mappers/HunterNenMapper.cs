using ClassLibrary.Core.DTOs;

namespace gRPC.HunterNenService.Mappers;

public static class HunterNenMapper
{
    public static HunterNenResponse ToGrpcResponse(HunterNenDTO dto)
    {
        return new HunterNenResponse
        {
            IdHunter = dto.Id_Hunter,
            IdNen = dto.Id_NenType,
            NenLevel = dto.NenLevel
        };
    }

    public static HunterNenDTO ToDTO(HunterNenRequest request)
    {
        return new HunterNenDTO
        {
            Id_Hunter = request.IdHunter,
            Id_NenType = request.IdNen,
            NenLevel = request.NenLevel
        };
    }
}