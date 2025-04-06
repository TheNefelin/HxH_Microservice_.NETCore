﻿using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace ClassLibrary.Infrastructure.Data;

public class OracleDbContext
{
    private readonly string _connectionString;
    private readonly ILogger<OracleDbContext> _logger;

    public OracleDbContext(string connectionString, ILogger<OracleDbContext> logger)
    {
        _connectionString = connectionString;
        _logger = logger;
    }

    public async Task<DataTable> ExecuteQueryAsync(string query, params OracleParameter[] parameters)
    {
        try
        {
            using var connection = new OracleConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new OracleCommand(query, connection);
            if (parameters.Length > 0)
                command.Parameters.AddRange(parameters);

            using var reader = await command.ExecuteReaderAsync();
            var resultTable = new DataTable();
            resultTable.Load(reader);

            _logger.LogInformation("[OracleDbContext] Query executed: {Query} with Parameters: {@Parameters}", query, parameters);
            return resultTable;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[OracleDbContext] Error in ExecuteNonQueryAsync with query: {Query} and Parameters: {@Parameters}", query, parameters);
            throw;
        }        
    }

    public async Task<int> ExecuteNonQueryAsync(string query, params OracleParameter[] parameters)
    {
        try
        {
            using var connection = new OracleConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new OracleCommand(query, connection);
            if (parameters.Length > 0)
                command.Parameters.AddRange(parameters);

            _logger.LogInformation("[OracleDbContext] Query execute correctly: {Query}", query);
            return await command.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[OracleDbContext] Error in ExecuteNonQueryAsync with query: {Query}", query);
            throw;
        }
    }
}
