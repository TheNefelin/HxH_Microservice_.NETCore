using ClassLibrary.Core.DTOs;
using ClassLibrary.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace ClassLibrary.Application.Services;

public class HunterService : IHunterService
{
    private readonly IHunterRepository _hunterRepository;
    private readonly ILogger<HunterService> _logger;

    public HunterService(IHunterRepository hunterRepository, ILogger<HunterService> logger)
    {
        _hunterRepository = hunterRepository;
        _logger = logger;
    }

    public async Task<bool> CreateAsync(HunterDTO hunterDTO)
    {
        _logger.LogInformation("[HunterService] Creating new hunter: {@Hunter}", hunterDTO);
        var result = await _hunterRepository.CreateAsync(hunterDTO);

        if (!result)
            _logger.LogWarning("[HunterService] Failed to create hunter: {@Hunter}", hunterDTO);

        return result;
    }

    public Task<bool> DeleteAsync(int id)
    {
        _logger.LogInformation("[HunterService] Deleting hunter with ID {Id}", id);
        return _hunterRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<HunterDTO>> GetAllAsync()
    {
        _logger.LogInformation("[HunterService] Retrieving all hunters");
        return await _hunterRepository.GetAllAsync();
    }

    public async Task<HunterDTO?> GetByIdAsync(int id)
    {
        _logger.LogInformation("[HunterService] Retrieving hunter by ID: {Id}", id);
        return await _hunterRepository.GetByIdAsync(id);
    }

    public Task<bool> UpdateAsync(HunterDTO hunterDTO)
    {
        _logger.LogInformation("[HunterService] Updating hunter: {@Hunter}", hunterDTO);
        return _hunterRepository.UpdateAsync(hunterDTO);
    }
}
