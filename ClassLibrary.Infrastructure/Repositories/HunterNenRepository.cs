using ClassLibrary.Core.Common;
using ClassLibrary.Core.DTOs;
using ClassLibrary.Core.Interfaces;
using ClassLibrary.Infrastructure.Data;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;
using System.Data;

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

    public async Task<IEnumerable<HunterNenDTO>> GetAllHunterNensAsync()
    {
        _logger.LogInformation("[HunterNenRepository] Retrieving all HunterNens");
        string query = "SELECT Id_Hunter, Id_Nen_Type, Nen_Level FROM Hunter_Nen";

        _logger.LogInformation("[HunterNenRepository] Executing query: {Query}", query);
        var dataTable = await _context.ExecuteQueryAsync(query);

        var listHunterNenDTO = new List<HunterNenDTO>();
        foreach (DataRow row in dataTable.Rows)
        {
            listHunterNenDTO.Add(new HunterNenDTO
            {
                Id_Hunter = Convert.ToInt32(row["ID_HUNTER"]),
                Id_NenType = Convert.ToInt32(row["ID_NEN_TYPE"]),
                NenLevel = Convert.ToInt32(row["NEN_LEVEL"]),
            });
        }

        return listHunterNenDTO;
    }

    public async Task<HunterNenDTO?> GetHunterNenByIdAsync(HunterNenDTO hunterNen)
    {
        _logger.LogInformation("[HunterNenRepository] Retrieving HunterNen by Id: {@HunterNen}", hunterNen);
        string query = "SELECT Id_Hunter, Id_Nen_Type, Nen_Level FROM Hunter_Nen WHERE Id_Hunter = :Id_Hunter AND Id_Nen_Type = :Id_NenType";

        var parameters = new[]
        {
            new OracleParameter("Id_Hunter", hunterNen.Id_Hunter),
            new OracleParameter("Id_NenType", hunterNen.Id_NenType)
        };

        _logger.LogInformation("[HunterNenRepository] Executing query: {Query} with parameters: {@Parameters}", query, parameters);
        var dataTable = await _context.ExecuteQueryAsync(query, parameters);

        if (dataTable.Rows.Count == 0)
            return null;

        var row = dataTable.Rows[0];

        var hunterNenDTO = new HunterNenDTO
        {
            Id_Hunter = Convert.ToInt32(row["ID_HUNTER"]),
            Id_NenType = Convert.ToInt32(row["ID_NEN_TYPE"]),
            NenLevel = Convert.ToInt32(row["NEN_LEVEL"]),
        };

        return hunterNenDTO;
    }

    public async Task<Result<bool>> InsertHunterNenAsync(HunterNenDTO hunterNen)
    {
        var validationResult = ValidateHunterNen(hunterNen);
        if (validationResult.IsFailure)
        {
            _logger.LogWarning("[HunterNenRepository] Validation failed: {Message}", validationResult.Message);
            return validationResult;
        }

        if (!await ExistsInTableAsync("Hunter", "Id_Hunter", hunterNen.Id_Hunter))
        {
            _logger.LogWarning("[HunterNenRepository] Hunter not found: {Id_Hunter}", hunterNen.Id_Hunter);
            return Result<bool>.Failure("Hunter not found.");
        }

        if (!await ExistsInTableAsync("Nen_Type", "Id_Nen_Type", hunterNen.Id_NenType))
        {
            _logger.LogWarning("[HunterNenRepository] NenType not found: {Id_NenType}", hunterNen.Id_NenType);
            return Result<bool>.Failure("NenType not found.");
        }

        if (await HunterNenExists(hunterNen))
        {
            _logger.LogInformation("[HunterNenRepository] HunterNen already exists: {@HunterNen}", hunterNen);
            return Result<bool>.Failure("HunterNen already exists.");
        }

        _logger.LogInformation("[HunterNenRepository] Adding HunterNen: {@HunterNen}", hunterNen);
        var query = "INSERT INTO Hunter_Nen (Id_Hunter, Id_Nen_Type, Nen_Level) VALUES (:Id_Hunter, :Id_NenType, :NenLevel)";

        var parameters = new[]
        {
            new OracleParameter("Id_Hunter", hunterNen.Id_Hunter),
            new OracleParameter("Id_NenType", hunterNen.Id_NenType),
            new OracleParameter("NenLevel", hunterNen.NenLevel),
        };

        _logger.LogInformation("[HunterNenRepository] Executing query: {Query} with parameters: {@Parameters}", query, parameters);
        var affectedRows = await _context.ExecuteNonQueryAsync(query, parameters);

        if (affectedRows > 0)
        {
            _logger.LogInformation("[HunterNenRepository] Insert successful.");
            return Result<bool>.Success(true, "Inserted successfully.");
        }

        _logger.LogError("[HunterNenRepository] Insert failed for: {@HunterNen}", hunterNen);
        return Result<bool>.Failure("Failed to insert HunterNen.");
    }

    public async Task<Result<bool>> UpdateHunterNenAsync(HunterNenDTO hunterNen)
    {
        var validationResult = ValidateHunterNen(hunterNen);
        if (validationResult.IsFailure)
        {
            _logger.LogWarning("[HunterNenRepository] HunterNenDTO Validation failed: {Message}", validationResult.Message);
            return validationResult;
        }

        if (!await ExistsInTableAsync("Hunter", "Id_Hunter", hunterNen.Id_Hunter))
        {
            _logger.LogWarning("[HunterNenRepository] Hunter not found: {Id_Hunter}", hunterNen.Id_Hunter);
            return Result<bool>.Failure("Hunter not found.");
        }

        if (!await ExistsInTableAsync("Nen_Type", "Id_Nen_Type", hunterNen.Id_NenType))
        {
            _logger.LogWarning("[HunterNenRepository] NenType not found: {Id_NenType}", hunterNen.Id_NenType);
            return Result<bool>.Failure("NenType not found.");
        }

        if (!await HunterNenExists(hunterNen))
        {
            _logger.LogInformation("[HunterNenRepository] HunterNen does not exists: {@HunterNen}", hunterNen);
            return Result<bool>.Failure("HunterNen already exists.");
        }

        _logger.LogInformation("[HunterNenRepository] Updating HunterNen: {@HunterNen}", hunterNen);

        string query = @"
        UPDATE Hunter_Nen 
        SET Nen_Level = :NenLevel 
        WHERE Id_Hunter = :Id_Hunter AND Id_Nen_Type = :Id_NenType";

        var parameters = new[]
        {
            new OracleParameter("NenLevel", hunterNen.NenLevel),
            new OracleParameter("Id_Hunter", hunterNen.Id_Hunter),
            new OracleParameter("Id_NenType", hunterNen.Id_NenType)
        };

        _logger.LogInformation("[HunterNenRepository] Executing query: {Query} with parameters: {@Parameters}", query, parameters);

        var affectedRows = await _context.ExecuteNonQueryAsync(query, parameters);
        
        if (affectedRows > 0)
        {
            _logger.LogInformation("[HunterNenRepository] Update successful.");
            return Result<bool>.Success(true, "Inserted successfully.");
        }

        _logger.LogError("[HunterNenRepository] Update failed for: {@HunterNen}", hunterNen);
        return Result<bool>.Failure("Failed to Update HunterNen.");
    }

    public async Task<Result<bool>> DeleteHunterNenAsync(HunterNenDTO hunterNen)
    {
        var validationResult = ValidateHunterNen(hunterNen);
        if (validationResult.IsFailure)
        {
            _logger.LogWarning("[HunterNenRepository] HunterNenDTO Validation failed: {Message}", validationResult.Message);
            return validationResult;
        }

        if (!await HunterNenExists(hunterNen))
        {
            _logger.LogInformation("[HunterNenRepository] HunterNen does not exists: {@HunterNen}", hunterNen);
            return Result<bool>.Failure("HunterNen already exists.");
        }

        _logger.LogInformation("[HunterNenRepository] Deleting HunterNen: {@HunterNen}", hunterNen);
        string query = "DELETE FROM Hunter_Nen WHERE Id_Hunter = :Id_Hunter AND Id_Nen_Type = :Id_NenType";

        var parameters = new[]
        {
            new OracleParameter("Id_Hunter", hunterNen.Id_Hunter),
            new OracleParameter("Id_NenType", hunterNen.Id_NenType)
        };

        _logger.LogInformation("[HunterNenRepository] Executing query: {Query} with parameters: {@Parameters}", query, parameters);
        var affectedRows = await _context.ExecuteNonQueryAsync(query, parameters);

        if (affectedRows > 0)
        {
            _logger.LogInformation("[HunterNenRepository] Delete successful.");
            return Result<bool>.Success(true, "Delete successfully.");
        }

        _logger.LogError("[HunterNenRepository] Delete failed for: {@HunterNen}", hunterNen);
        return Result<bool>.Failure("Failed to Delete HunterNen.");
    }

    private async Task<bool> ExistsInTableAsync(string table, string column, int value)
    {
        var query = $"SELECT COUNT(1) FROM {table} WHERE {column} = :value";
        var param = new OracleParameter("value", value);
        var count = await _context.ExecuteScalarAsync<int>(query, param);
        return count > 0;
    }

    private async Task<bool> HunterNenExists(HunterNenDTO hunterNen)
    {
        var query = "SELECT COUNT(1) FROM Hunter_Nen WHERE Id_Hunter = :Id_Hunter AND Id_Nen_Type = :Id_NenType";
        var parameters = new[]
        {
            new OracleParameter("Id_Hunter", hunterNen.Id_Hunter),
            new OracleParameter("Id_NenType", hunterNen.Id_NenType)
        };

        var count = await _context.ExecuteScalarAsync<int>(query, parameters);
        return count > 0;
    }

    private Result<bool> ValidateHunterNen(HunterNenDTO hunterNen)
    {
        if (hunterNen == null)
            return Result<bool>.Failure("HunterNen cannot be null");

        if (hunterNen.Id_Hunter <= 0)
            return Result<bool>.Failure("Id_Hunter must be greater than 0");

        if (hunterNen.Id_NenType <= 0)
            return Result<bool>.Failure("Id_NenType must be greater than 0");

        if (hunterNen.NenLevel < 1 || hunterNen.NenLevel > 100)
            return Result<bool>.Failure("NenLevel must be between 1 and 100");

        return Result<bool>.Success(true);
    }
}
