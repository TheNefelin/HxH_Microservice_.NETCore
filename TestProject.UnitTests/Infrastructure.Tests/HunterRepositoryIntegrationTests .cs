using ClassLibrary.Core.DTOs;
using ClassLibrary.Infrastructure.Repositories;
using Microsoft.Extensions.Logging.Abstractions;

namespace TestProject.UnitTests.Infrastructure.Tests;

[Collection("Database collection")]
public class HunterRepositoryIntegrationTests
{
    private readonly HunterRepository _hunterRepository;

    public HunterRepositoryIntegrationTests(DatabaseFixture fixture)
    {
        var logger = NullLogger<HunterRepository>.Instance;
        _hunterRepository = new HunterRepository(fixture.DbContext, logger);
    }

    [Fact]
    public async Task CreateAsync_ShouldInsertHunter()
    {
        // Arrange
        var hunter = new HunterDTO { Name = "Gon", Age = 12, Origin = "Whale Island" };

        // Act
        var result = await _hunterRepository.CreateAsync(hunter);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllHunters()
    {
        // Act
        var result = await _hunterRepository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnCorrectHunter()
    {
        // Arrange
        var hunter = new HunterDTO { Name = "Killua", Age = 12, Origin = "Zoldyck" };
        await _hunterRepository.CreateAsync(hunter);

        var all = await _hunterRepository.GetAllAsync();
        var lastHunter = all.Last();

        // Act
        var result = await _hunterRepository.GetByIdAsync(lastHunter.Id_Hunter);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(hunter.Name, result.Name);
    }
}
