using ClassLibrary.Core.DTOs;

namespace ClassLibrary.Core.Interfaces;

public interface INenService
{
    Task<IEnumerable<NenTypeDTO>> GetAllAsync();
    Task<NenTypeDTO?> GetByIdAsync(int id);
    Task<bool> CreateAsync(NenTypeDTO nenType);
    Task<bool> UpdateAsync(NenTypeDTO nenType);
    Task<bool> DeleteAsync(int id);
}
