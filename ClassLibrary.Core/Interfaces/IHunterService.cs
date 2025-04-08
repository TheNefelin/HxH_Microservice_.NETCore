using ClassLibrary.Core.DTOs;

namespace ClassLibrary.Core.Interfaces;

public interface IHunterService
{
    Task<IEnumerable<HunterDTO>> GetAllAsync();
    Task<HunterDTO?> GetByIdAsync(int id);
    Task<int> CreateAsync(HunterDTO hunterDTO);
    Task<bool> UpdateAsync(HunterDTO hunterDTO);
    Task<bool> DeleteAsync(int id);
}
