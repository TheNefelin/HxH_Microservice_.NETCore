using ClassLibrary.Core.DTOs;
using ClassLibrary.Core.Interfaces;
using ClassLibrary.Infrastructure.Data;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;

namespace ClassLibrary.Infrastructure.Repositories;

public class HunterNenRepository : IHunterNenRepository
{
    private readonly ILogger<HunterNenRepository> _logger;
    private readonly OracleDbContext _context;

    public HunterNenRepository(ILogger<HunterNenRepository> logger, OracleDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<bool> AddHunterNenAsync(HunterNenDTO hunterNen)
    {
        if (hunterNen == null)
            throw new ArgumentNullException(nameof(hunterNen), "HunterNen cannot be null");

        var checkQuery = "SELECT COUNT(1) FROM Hunter_Nen WHERE Id_Hunter = :Id_Hunter AND Id_Nen_Type = :Id_NenType";
        var checkParameters = new[]
        {
            new OracleParameter("Id_Hunter", hunterNen.Id_Hunter),
            new OracleParameter("Id_NenType", hunterNen.Id_NenType)
        };

        try
        {
            var exists = await _context.ExecuteScalarAsync<int>(checkQuery, checkParameters);
            if (exists > 0)
            {
                _logger.LogInformation("[HunterNenRepository] HunterNen already exists: {@HunterNen}", hunterNen);
                return true; 
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[HunterNenRepository] Error checking for existing HunterNen: {@HunterNen}", hunterNen);
            return false;
        }

        _logger.LogInformation("[HunterNenRepository] Adding HunterNen: {@HunterNen}", hunterNen);
        var query = "INSERT INTO Hunter_Nen (Id_Hunter, Id_Nen_Type, Nen_Level) VALUES (:Id_Hunter, :Id_NenType, :NenLevel)";

        var parameters = new[]
        {
            new OracleParameter("Id_Hunter", hunterNen.Id_Hunter),
            new OracleParameter("Id_NenType", hunterNen.Id_NenType),
            new OracleParameter("NenLevel", hunterNen.NenLevel)
        };

        try
        {
            _logger.LogInformation("[HunterNenRepository] Executing query: {Query} with parameters: {@Parameters}", query, parameters);
            var affectedRows = await _context.ExecuteNonQueryAsync(query, parameters);
            return affectedRows > 0;
        }
        catch (OracleException ex)
        {
            _logger.LogError(ex, "[HunterNenRepository] Oracle error adding HunterNen: {@HunterNen}", hunterNen);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[HunterNenRepository] Unexpected error adding HunterNen: {@HunterNen}", hunterNen);
            return false;
        }
    }

    public Task<bool> DeleteHunterNenAsync(int idHunter, int idNenType)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<HunterNenDTO>> GetAllHunterNensAsync()
    {
        throw new NotImplementedException();
    }

    public Task<HunterNenDTO?> GetHunterNenByIdAsync(int idHunter, int idNenType)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateHunterNenAsync(HunterNenDTO hunterNen)
    {
        throw new NotImplementedException();
    }
}
