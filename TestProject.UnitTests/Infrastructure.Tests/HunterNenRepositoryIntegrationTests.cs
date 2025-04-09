using ClassLibrary.Core.DTOs;
using ClassLibrary.Infrastructure.Repositories;
using Microsoft.Extensions.Logging.Abstractions;

namespace TestProject.UnitTests.Infrastructure.Tests;

[Collection("Database collection")]
public class HunterNenRepositoryIntegrationTests
{
    private readonly HunterNenRepository _hunterNenRepository;

    public HunterNenRepositoryIntegrationTests(DatabaseFixture fixture)
    {
        var logger = NullLogger<HunterNenRepository>.Instance;
        _hunterNenRepository = new HunterNenRepository(logger, fixture.DbContext);
    }

    [Fact]
    public async Task AddHunterNenAsync_ShouldInsertHunterNen()
    {
        // Arrange
        var hunterNen = new HunterNenDTO
        {
            Id_Hunter = 1,
            Id_NenType = 1,
            NenLevel = 50.0f
        };

        // Act
        var result = await _hunterNenRepository.AddHunterNenAsync(hunterNen);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task AddHunterNenAsync_ShouldThrowArgumentNullException_WhenHunterNenIsNull()
    {
        // Arrange
        HunterNenDTO hunterNen = null;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _hunterNenRepository.AddHunterNenAsync(hunterNen));
    }

    [Fact]
    public async Task AddHunterNenAsync_ShouldReturnFalse_WhenInsertFails()
    {
        // Arrange
        var hunterNen = new HunterNenDTO
        {
            Id_Hunter = 9999, // Assuming this ID does not exist in the database
            Id_NenType = 9999,
            NenLevel = 50.0f
        };

        // Act
        var result = await _hunterNenRepository.AddHunterNenAsync(hunterNen);

        // Assert
        Assert.False(result);
    }
}
