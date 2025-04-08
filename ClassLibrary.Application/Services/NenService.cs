using ClassLibrary.Core.DTOs;
using ClassLibrary.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace ClassLibrary.Application.Services;

public class NenService : INenService
{
    private readonly ILogger<NenService> _logger;
    private readonly INenRepository _nenRepository;

    public NenService(ILogger<NenService> logger, INenRepository nenRepository)
    {
        _logger = logger;
        _nenRepository = nenRepository;
    }

    public Task<int> CreateAsync(NenTypeDTO nenType)
    {
        _logger.LogInformation("[NenService] Creating new NenType: {@NenType}", nenType);
        return _nenRepository.CreateAsync(nenType);
    }

    public Task<bool> DeleteAsync(int id)
    {
        _logger.LogInformation("[NenService] Deleting NenType with ID {Id}", id);
        return _nenRepository.DeleteAsync(id);
    }

    public Task<IEnumerable<NenTypeDTO>> GetAllAsync()
    {
        _logger.LogInformation("[NenService] Retrieving all NenTypes");
        return _nenRepository.GetAllAsync();
    }

    public Task<NenTypeDTO?> GetByIdAsync(int id)
    {
        _logger.LogInformation("[NenService] Retrieving NenType by ID: {Id}", id);
        return _nenRepository.GetByIdAsync(id);
    }

    public Task<bool> UpdateAsync(NenTypeDTO nenType)
    {
        _logger.LogInformation("[NenService] Updating NenType: {@NenType}", nenType);
        return _nenRepository.UpdateAsync(nenType);
    }
}
