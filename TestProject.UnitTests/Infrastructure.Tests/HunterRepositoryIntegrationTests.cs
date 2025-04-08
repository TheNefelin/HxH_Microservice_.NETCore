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
        var hunter = new HunterDTO { Name = "Hisoka Morow", Age = 28, Origin = "Ciudad desconocida" };

        // Act
        var idHunter = await _hunterRepository.CreateAsync(hunter);

        // Assert
        Assert.True(idHunter > 0);
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
        var hunter = new HunterDTO { Name = "Isaac Netero", Age = 110, Origin = "Ciudad desconocida" };
        var idHunter = await _hunterRepository.CreateAsync(hunter);

        // Act
        var result = await _hunterRepository.GetByIdAsync(idHunter);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(hunter.Name, result.Name);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveHunter()
    {
        // Arrange
        var hunter = new HunterDTO { Name = "Biscuit Krueger", Age = 57, Origin = "Ciudad desconocida" };
        await _hunterRepository.CreateAsync(hunter);
        var all = await _hunterRepository.GetAllAsync();
        var lastHunter = all.Last();

        // Act
        var result = await _hunterRepository.DeleteAsync(lastHunter.Id_Hunter);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task UpdateAsync_ShouldModifyHunter()
    {
        // Arrange
        var hunter = new HunterDTO { Name = "Chrollo", Age = 26, Origin = "Ciudad Meteoro" };
        await _hunterRepository.CreateAsync(hunter);
        var all = await _hunterRepository.GetAllAsync();
        var lastHunter = all.Last();

        // Act
        lastHunter.Name = "Chrollo Lucilfer";
        var result = await _hunterRepository.UpdateAsync(lastHunter);
        
        // Assert
        Assert.True(result);
    }
}
