using ClassLibrary.Core.Common;
using ClassLibrary.Core.DTOs;

namespace ClassLibrary.Core.Interfaces;

public interface IHunterNenRepository
{
    Task<IEnumerable<HunterNenDTO>> GetAllHunterNensAsync();
    Task<HunterNenDTO?> GetHunterNenByIdAsync(HunterNenDTO hunterNen);
    Task<Result<bool>> InsertHunterNenAsync(HunterNenDTO hunterNen);
    Task<Result<bool>> UpdateHunterNenAsync(HunterNenDTO hunterNen);
    Task<Result<bool>> DeleteHunterNenAsync(HunterNenDTO hunterNen);
}
