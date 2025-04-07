using ClassLibrary.Infrastructure.Data;
using Microsoft.Extensions.Logging.Abstractions;

namespace TestProject.UnitTests.Infrastructure.Tests;

public class DatabaseFixture : IDisposable
{
    public OracleDbContext DbContext { get; }

    public DatabaseFixture()
    {
        var connectionString = "User Id=testing;Password=testing;Data Source=localhost:1521/xe";
        var logger = NullLogger<OracleDbContext>.Instance;
        DbContext = new OracleDbContext(connectionString, logger);
    }

    public void Dispose()
    {

    }
}

