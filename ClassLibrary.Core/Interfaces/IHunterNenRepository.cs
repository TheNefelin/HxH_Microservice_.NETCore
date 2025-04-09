using ClassLibrary.Core.DTOs;

namespace ClassLibrary.Core.Interfaces;

public interface IHunterNenRepository
{
    Task<bool> AddHunterNenAsync(HunterNenDTO hunterNen);
    Task<bool> UpdateHunterNenAsync(HunterNenDTO hunterNen);
    Task<bool> DeleteHunterNenAsync(int idHunter, int idNenType);
    Task<HunterNenDTO?> GetHunterNenByIdAsync(int idHunter, int idNenType);
    Task<IEnumerable<HunterNenDTO>> GetAllHunterNensAsync();
}
