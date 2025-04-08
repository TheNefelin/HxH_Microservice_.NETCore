using ClassLibrary.Core.DTOs;
using ClassLibrary.Core.Interfaces;
using ClassLibrary.Infrastructure.Data;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Data;

namespace ClassLibrary.Infrastructure.Repositories;

public class NenRepository : INenRepository
{
    private readonly OracleDbContext _dbContext;
    private readonly ILogger<NenRepository> _logger;

    public NenRepository(OracleDbContext dbContext, ILogger<NenRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<int> CreateAsync(NenTypeDTO nenType)
    {
        if (nenType == null)
            throw new ArgumentNullException(nameof(nenType), "NenTypeDTO cannot be null");

        _logger.LogInformation("[NenRepository] Creating new NenType: {@NenType}", nenType);
        string query = @"
        INSERT INTO NEN_TYPE (NAME, DESCRIPTION)
        VALUES (:name, : description)
        RETURNING ID_NEN_TYPE INTO :id";

        var idParam = new OracleParameter("id", OracleDbType.Int32)
        {
            Direction = ParameterDirection.Output
        };

        var parameters = new[]
        {
            new OracleParameter("name", nenType.Name),
            new OracleParameter("description", nenType.Description),
            idParam
        };

        var affectedRows = await _dbContext.ExecuteNonQueryAsync(query, parameters);
        _logger.LogInformation("[NenRepository] Rows affected: {Rows}", affectedRows);

        return affectedRows > 0 ? Convert.ToInt32(((OracleDecimal)idParam.Value).ToInt32()) : 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        _logger.LogInformation("[NenRepository] Deleting NenType with ID: {Id}", id);
        string query = @"DELETE FROM NEN_TYPE WHERE ID_NEN_TYPE = :id";

        var idParam = new OracleParameter("id", id);

        var affectedRows = await _dbContext.ExecuteNonQueryAsync(query, idParam);
        _logger.LogInformation("[NenRepository] Rows affected: {Rows}", affectedRows);

        return affectedRows > 0;
    }

    public async Task<IEnumerable<NenTypeDTO>> GetAllAsync()
    {
        _logger.LogInformation("[NenRepository] Retrieving all NenTypes");
        string query = @"
        SELECT ID_NEN_TYPE, NAME, DESCRIPTION FROM NEN_TYPE";

        var table = await _dbContext.ExecuteQueryAsync(query);

        var listNenType = new List<NenTypeDTO>();
        foreach (DataRow row in table.Rows)
        {
            var nenType = new NenTypeDTO
            {
                Id_NenType = Convert.ToInt32(row["ID_NEN_TYPE"]),
                Name = row["NAME"].ToString(),
                Description = row["DESCRIPTION"].ToString()
            };

            listNenType.Add(nenType);
        }

        _logger.LogInformation("[NenRepository] NenTypes retrieved: {@listNenType}", listNenType);
        return listNenType;
    }

    public async Task<NenTypeDTO?> GetByIdAsync(int id)
    {
        _logger.LogInformation("[NenRepository] Retrieving NenType with ID: {Id}", id);
        string query = @"
        SELECT ID_NEN_TYPE, NAME, DESCRIPTION FROM NEN_TYPE WHERE ID_NEN_TYPE = :id";

        var idParam = new OracleParameter("id", id);

        var table = await _dbContext.ExecuteQueryAsync(query, idParam);

        if (table.Rows.Count == 0)
        {
            _logger.LogWarning("[NenRepository] NenType with ID: {Id} not found", id);
            return null;
        }

        var nenType = new NenTypeDTO
        {
            Id_NenType = Convert.ToInt32(table.Rows[0]["ID_NEN_TYPE"]),
            Name = table.Rows[0]["NAME"].ToString(),
            Description = table.Rows[0]["DESCRIPTION"].ToString()
        };

        _logger.LogInformation("[NenRepository] NenType retrieved: {@NenType}", nenType);
        return nenType;
    }

    public async Task<bool> UpdateAsync(NenTypeDTO nenType)
    {
        _logger.LogInformation("[NenRepository] Updating NenType: {@NenType}", nenType);

        string query = @"DELETE FROM NEN_TYPE WHERE ID_NEN_TYPE = :id";
      
        var parameters = new[]
        {
            new OracleParameter("name", nenType.Name),
            new OracleParameter("description", nenType.Description),
        };

        var affectedRows = await _dbContext.ExecuteNonQueryAsync(query, parameters);
        _logger.LogInformation("[NenRepository] Rows affected: {Rows}", affectedRows);

        return affectedRows > 0;
    }
}
