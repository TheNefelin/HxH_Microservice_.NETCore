using ClassLibrary.Core.DTOs;

namespace ClassLibrary.Core.Interfaces;

public interface INenRepository
{
    Task<IEnumerable<NenTypeDTO>> GetAllAsync();
    Task<NenTypeDTO?> GetByIdAsync(int id);
    Task<int> CreateAsync(NenTypeDTO nenType);
    Task<bool> UpdateAsync(NenTypeDTO nenType);
    Task<bool> DeleteAsync(int id);
}
