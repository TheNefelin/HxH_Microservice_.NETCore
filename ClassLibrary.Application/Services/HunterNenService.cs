using ClassLibrary.Core.Common;
using ClassLibrary.Core.DTOs;
using ClassLibrary.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace ClassLibrary.Application.Services;

public class HunterNenService : IHunterNenService
{
    private readonly ILogger<HunterService> _logger;
    private readonly IHunterNenRepository _hunterNenRepository;

    public HunterNenService(ILogger<HunterService> logger, IHunterNenRepository hunterNenRepository)
    {
        _logger = logger;
        _hunterNenRepository = hunterNenRepository;
    }

    public async Task<IEnumerable<HunterNenDTO>> GetAllHunterNensAsync()
    {
        _logger.LogInformation("[HunterNenService] Retrieving all HunterNens");
        return await _hunterNenRepository.GetAllHunterNensAsync();
    }

    public async Task<HunterNenDTO?> GetHunterNenByIdAsync(HunterNenDTO hunterNen)
    {
        _logger.LogInformation("[HunterNenService] Retrieving HunterNen by ID: {@HunterNen}", hunterNen);
        return await _hunterNenRepository.GetHunterNenByIdAsync(hunterNen);
    }

    public async Task<Result<bool>> InsertHunterNenAsync(HunterNenDTO hunterNen)
    {
        _logger.LogInformation("[HunterNenService] Inserting new HunterNen: {@HunterNen}", hunterNen);
        return await _hunterNenRepository.InsertHunterNenAsync(hunterNen);
    }

    public Task<Result<bool>> UpdateHunterNenAsync(HunterNenDTO hunterNen)
    {
        _logger.LogInformation("[HunterNenService] Updating HunterNen: {@HunterNen}", hunterNen);
        return _hunterNenRepository.UpdateHunterNenAsync(hunterNen);
    }

    public Task<Result<bool>> DeleteHunterNenAsync(HunterNenDTO hunterNen)
    {
        _logger.LogInformation("[HunterNenService] Deleting HunterNen: {@HunterNen}", hunterNen);
        return _hunterNenRepository.DeleteHunterNenAsync(hunterNen);
    }
}
