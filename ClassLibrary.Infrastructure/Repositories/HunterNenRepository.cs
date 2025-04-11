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

    // GET ALL
    public async Task<IEnumerable<HunterNenDTO>> GetAllHunterNensAsync()
    {
        _logger.LogInformation("Retrieving all HunterNens");
        const string query = "SELECT Id_Hunter, Id_Nen_Type, Nen_Level FROM Hunter_Nen";

        var dataTable = await _context.ExecuteQueryAsync(query);

        var result = new List<HunterNenDTO>();
        foreach (DataRow row in dataTable.Rows)
        {
            result.Add(new HunterNenDTO
            {
                Id_Hunter = Convert.ToInt32(row["ID_HUNTER"]),
                Id_NenType = Convert.ToInt32(row["ID_NEN_TYPE"]),
                NenLevel = Convert.ToInt32(row["NEN_LEVEL"]),
            });
        }

        return result;
    }

    // GET BY ID
    public async Task<HunterNenDTO?> GetHunterNenByIdAsync(HunterNenDTO hunterNen)
    {
        _logger.LogInformation("Retrieving HunterNen by Id: {@HunterNen}", hunterNen);

        const string query = "SELECT Id_Hunter, Id_Nen_Type, Nen_Level FROM Hunter_Nen WHERE Id_Hunter = :Id_Hunter AND Id_Nen_Type = :Id_NenType";

        var parameters = new[]
        {
            new OracleParameter("Id_Hunter", hunterNen.Id_Hunter),
            new OracleParameter("Id_NenType", hunterNen.Id_NenType)
        };

        var dataTable = await _context.ExecuteQueryAsync(query, parameters);

        if (dataTable.Rows.Count == 0)
            return null;

        var row = dataTable.Rows[0];
        return new HunterNenDTO
        {
            Id_Hunter = Convert.ToInt32(row["ID_HUNTER"]),
            Id_NenType = Convert.ToInt32(row["ID_NEN_TYPE"]),
            NenLevel = Convert.ToInt32(row["NEN_LEVEL"]),
        };
    }

    // INSERT
    public async Task<Result<bool>> InsertHunterNenAsync(HunterNenDTO hunterNen)
    {
        var validationResult = ValidateHunterNen(hunterNen);
        if (validationResult.IsFailure)
        {
            _logger.LogWarning("Validation failed: {Message}", validationResult.Message);
            return validationResult;
        }

        var foreignKeyResult = await ValidateForeignKeysAsync(hunterNen);
        if (foreignKeyResult.IsFailure)
        {
            _logger.LogWarning("Foreign key validation failed: {Message}", foreignKeyResult.Message);
            return foreignKeyResult;
        }

        if (await HunterNenExists(hunterNen))
        {
            _logger.LogInformation("HunterNen already exists: {@HunterNen}", hunterNen);
            return Result<bool>.Failure("HunterNen already exists.");
        }

        const string query = "INSERT INTO Hunter_Nen (Id_Hunter, Id_Nen_Type, Nen_Level) VALUES (:Id_Hunter, :Id_NenType, :NenLevel)";

        var parameters = new[]
        {
            new OracleParameter("Id_Hunter", hunterNen.Id_Hunter),
            new OracleParameter("Id_NenType", hunterNen.Id_NenType),
            new OracleParameter("NenLevel", hunterNen.NenLevel),
        };

        var affectedRows = await _context.ExecuteNonQueryAsync(query, parameters);

        if (affectedRows > 0)
        {
            _logger.LogInformation("Insert successful.");
            return Result<bool>.Success(true, "Inserted successfully.");
        }

        _logger.LogError("Insert failed for: {@HunterNen}", hunterNen);
        return Result<bool>.Failure("Failed to insert HunterNen.");
    }

    // UPDATE
    public async Task<Result<bool>> UpdateHunterNenAsync(HunterNenDTO hunterNen)
    {
        var validationResult = ValidateHunterNen(hunterNen);
        if (validationResult.IsFailure)
        {
            _logger.LogWarning("Validation failed: {Message}", validationResult.Message);
            return validationResult;
        }

        var foreignKeyResult = await ValidateForeignKeysAsync(hunterNen);
        if (foreignKeyResult.IsFailure)
        {
            _logger.LogWarning("Foreign key validation failed: {Message}", foreignKeyResult.Message);
            return foreignKeyResult;
        }

        if (!await HunterNenExists(hunterNen))
        {
            _logger.LogInformation("HunterNen does not exist: {@HunterNen}", hunterNen);
            return Result<bool>.Failure("HunterNen does not exist.");
        }

        const string query = @"
            UPDATE Hunter_Nen 
            SET Nen_Level = :NenLevel 
            WHERE Id_Hunter = :Id_Hunter AND Id_Nen_Type = :Id_NenType";

        var parameters = new[]
        {
            new OracleParameter("NenLevel", hunterNen.NenLevel),
            new OracleParameter("Id_Hunter", hunterNen.Id_Hunter),
            new OracleParameter("Id_NenType", hunterNen.Id_NenType)
        };

        var affectedRows = await _context.ExecuteNonQueryAsync(query, parameters);

        if (affectedRows > 0)
        {
            _logger.LogInformation("Update successful.");
            return Result<bool>.Success(true, "Updated successfully.");
        }

        _logger.LogError("Update failed for: {@HunterNen}", hunterNen);
        return Result<bool>.Failure("Failed to update HunterNen.");
    }

    // DELETE
    public async Task<Result<bool>> DeleteHunterNenAsync(HunterNenDTO hunterNen)
    {
        var validationResult = ValidateHunterNen(hunterNen);
        if (validationResult.IsFailure)
        {
            _logger.LogWarning("Validation failed: {Message}", validationResult.Message);
            return validationResult;
        }

        if (!await HunterNenExists(hunterNen))
        {
            _logger.LogInformation("HunterNen does not exist: {@HunterNen}", hunterNen);
            return Result<bool>.Failure("HunterNen does not exist.");
        }

        const string query = "DELETE FROM Hunter_Nen WHERE Id_Hunter = :Id_Hunter AND Id_Nen_Type = :Id_NenType";

        var parameters = new[]
        {
            new OracleParameter("Id_Hunter", hunterNen.Id_Hunter),
            new OracleParameter("Id_NenType", hunterNen.Id_NenType)
        };

        var affectedRows = await _context.ExecuteNonQueryAsync(query, parameters);

        if (affectedRows > 0)
        {
            _logger.LogInformation("Delete successful.");
            return Result<bool>.Success(true, "Deleted successfully.");
        }

        _logger.LogError("Delete failed for: {@HunterNen}. AffectedRows: {AffectedRows}", hunterNen, affectedRows);
        return Result<bool>.Failure("Failed to delete HunterNen.");
    }

    // HELPERS
    private async Task<bool> HunterNenExists(HunterNenDTO hunterNen)
    {
        const string query = "SELECT COUNT(1) FROM Hunter_Nen WHERE Id_Hunter = :Id_Hunter AND Id_Nen_Type = :Id_NenType";
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

    private async Task<Result<bool>> ValidateForeignKeysAsync(HunterNenDTO hunterNen)
    {
        var hunterResult = await CheckExistence("Hunter", "Id_Hunter", hunterNen.Id_Hunter);
        if (hunterResult.IsFailure)
        {
            _logger.LogWarning("Hunter not found: {Message}", hunterResult.Message);
            return hunterResult;
        }

        var nenResult = await CheckExistence("Nen_Type", "Id_Nen_Type", hunterNen.Id_NenType);
        if (nenResult.IsFailure)
        {
            _logger.LogWarning("NenType not found: {Message}", nenResult.Message);
            return nenResult;
        }

        return Result<bool>.Success(true);
    }

    private async Task<Result<bool>> CheckExistence(string table, string column, int value)
    {
        var allowedTables = new[] { "Hunter", "Nen_Type" };
        if (!allowedTables.Contains(table))
            return Result<bool>.Failure("Invalid table name");

        var query = $"SELECT COUNT(1) FROM {table} WHERE {column} = :value";
        var param = new OracleParameter("value", value);
        var count = await _context.ExecuteScalarAsync<int>(query, param);

        return count > 0
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"{table} with {column} = {value} not found.");
    }
}