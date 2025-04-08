using ClassLibrary.Core.DTOs;
using ClassLibrary.Core.Interfaces;
using ClassLibrary.Infrastructure.Data;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Data;

namespace ClassLibrary.Infrastructure.Repositories;

public class HunterRepository : IHunterRepository
{
    private readonly OracleDbContext _dbContext;
    private readonly ILogger<HunterRepository> _logger;

    public HunterRepository(OracleDbContext dbContext, ILogger<HunterRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<int> CreateAsync(HunterDTO hunterDTO)
    {
        _logger.LogInformation("[HunterRepository] Creating new hunter: {@Hunter}", hunterDTO);
        string query = @"
        INSERT INTO HUNTER (NAME, AGE, ORIGIN) 
        VALUES (:name, :age, :origin) 
        RETURNING ID_HUNTER INTO :id";

        var idParam = new OracleParameter("id", OracleDbType.Int32)
        {
            Direction = ParameterDirection.Output
        };

        var parameters = new[]
        {
            new OracleParameter("name", hunterDTO.Name),
            new OracleParameter("age", hunterDTO.Age),
            new OracleParameter("origin", hunterDTO.Origin),
            idParam
        };

        var affectedRows = await _dbContext.ExecuteNonQueryAsync(query, parameters);
        _logger.LogInformation("[HunterRepository] Rows affected: {Rows}", affectedRows);

        return affectedRows > 0 ? Convert.ToInt32(((OracleDecimal)idParam.Value).ToInt32()) : 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        _logger.LogInformation("[HunterRepository] Deleting hunter with ID: {Id}", id);
        string query = @"
        DELETE FROM HUNTER WHERE ID_HUNTER = :id";

        var parameter = new OracleParameter("id", id);

        var affectedRows = await _dbContext.ExecuteNonQueryAsync(query, parameter);
        _logger.LogInformation("[HunterRepository] Rows affected: {Rows}", affectedRows);

        return affectedRows > 0;
    }

    public async Task<IEnumerable<HunterDTO>> GetAllAsync()
    {
        _logger.LogInformation("[HunterRepository] Retrieving all hunters...");
        string query = @"
        SELECT ID_HUNTER, NAME, AGE, ORIGIN FROM HUNTER";

        var table = await _dbContext.ExecuteQueryAsync(query);

        var list = new List<HunterDTO>();
        foreach (DataRow row in table.Rows)
        {
            list.Add(new HunterDTO
            {
                Id_Hunter = Convert.ToInt32(row["ID_HUNTER"]),
                Name = row["NAME"].ToString()!,
                Age = Convert.ToInt32(row["AGE"]),
                Origin = row["ORIGIN"].ToString()!
            });
        }

        _logger.LogInformation("[HunterRepository] Retrieved {Count} hunters", list.Count);
        return list;
    }

    public async Task<HunterDTO?> GetByIdAsync(int id)
    {
        _logger.LogInformation("[HunterRepository] Retrieving hunter by ID: {Id}", id);
        string query = @"
        SELECT ID_HUNTER, NAME, AGE, ORIGIN 
        FROM HUNTER 
        WHERE ID_HUNTER = :id";

        var parameter = new OracleParameter("id", id);

        var table = await _dbContext.ExecuteQueryAsync(query, parameter);

        if (table.Rows.Count == 0)
        {
            _logger.LogWarning("[HunterRepository] No hunter found with ID: {Id}", id);
            return null;
        }

        var row = table.Rows[0];
        var hunter = new HunterDTO
        {
            Id_Hunter = Convert.ToInt32(row["ID_HUNTER"]),
            Name = row["NAME"].ToString()!,
            Age = Convert.ToInt32(row["AGE"]),
            Origin = row["ORIGIN"].ToString()!
        };

        _logger.LogInformation("[HunterRepository] Hunter found: {@Hunter}", hunter);
        return hunter;
    }

    public async Task<bool> UpdateAsync(HunterDTO hunterDTO)
    {
        _logger.LogInformation("[HunterRepository] Updating hunter: {@Hunter}", hunterDTO);
        string query = @"
        UPDATE HUNTER 
        SET NAME = :name, AGE = :age, ORIGIN = :origin 
        WHERE ID_HUNTER = :id";

        var parameters = new[]
        {
            new OracleParameter("name", hunterDTO.Name),
            new OracleParameter("age", hunterDTO.Age),
            new OracleParameter("origin", hunterDTO.Origin),
            new OracleParameter("id", hunterDTO.Id_Hunter)
        };

        var affectedRows = await _dbContext.ExecuteNonQueryAsync(query, parameters);
        _logger.LogInformation("[HunterRepository] Rows affected: {Rows}", affectedRows);

        return affectedRows > 0;
    }
}
