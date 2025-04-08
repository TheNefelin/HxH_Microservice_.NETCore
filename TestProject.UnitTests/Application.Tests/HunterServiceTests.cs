using ClassLibrary.Application.Services;
using ClassLibrary.Core.DTOs;
using ClassLibrary.Core.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestProject.UnitTests.Application.Tests;

public class HunterServiceTests
{
    private readonly Mock<IHunterRepository> _mockRepo;
    private readonly Mock<ILogger<HunterService>> _mockLogger;
    private readonly HunterService _hunterService;

    public HunterServiceTests()
    {
        _mockRepo = new Mock<IHunterRepository>();
        _mockLogger = new Mock<ILogger<HunterService>>();
        _hunterService = new HunterService(_mockRepo.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnTrue_WhenCreationSucceeds()
    {
        // Arrange
        var hunter = new HunterDTO { Id_Hunter = 1, Name = "Gon", Age = 12, Origin = "Whale Island" };
        _mockRepo.Setup(r => r.CreateAsync(hunter)).ReturnsAsync(hunter.Id_Hunter);

        // Act
        var idHunter = await _hunterService.CreateAsync(hunter);

        // Assert
        Assert.Equal(idHunter, hunter.Id_Hunter);
        _mockRepo.Verify(r => r.CreateAsync(hunter), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnFalse_WhenCreationFails()
    {
        // Arrange
        var hunter = new HunterDTO { Id_Hunter = 2, Name = "Killua", Age = 12, Origin = "Zoldyck" };
        _mockRepo.Setup(r => r.CreateAsync(hunter)).ReturnsAsync(hunter.Id_Hunter + 1);

        // Act
        var idHunter = await _hunterService.CreateAsync(hunter);

        // Assert
        Assert.NotEqual(hunter.Id_Hunter, idHunter);
        _mockRepo.Verify(r => r.CreateAsync(hunter), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnTrue_WhenDeletionSucceeds()
    {
        // Arrange
        int hunterId = 1;
        _mockRepo.Setup(r => r.DeleteAsync(hunterId)).ReturnsAsync(true);

        // Act
        var result = await _hunterService.DeleteAsync(hunterId);

        // Assert
        Assert.True(result);
        _mockRepo.Verify(r => r.DeleteAsync(hunterId), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_WhenDeletionFails()
    {
        // Arrange
        int hunterId = 2;
        _mockRepo.Setup(r => r.DeleteAsync(hunterId)).ReturnsAsync(false);

        // Act
        var result = await _hunterService.DeleteAsync(hunterId);
        
        // Assert
        Assert.False(result);
        _mockRepo.Verify(r => r.DeleteAsync(hunterId), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllHunters()
    {
        // Arrange
        var hunters = new List<HunterDTO>
        {
            new HunterDTO { Id_Hunter = 1, Name = "Gon", Age = 12, Origin = "Whale Island" },
            new HunterDTO { Id_Hunter = 2, Name = "Killua", Age = 12, Origin = "Zoldyck" }
        };
        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(hunters);
        
        // Act
        var result = await _hunterService.GetAllAsync();
        
        // Assert
        Assert.Equal(2, result.Count());
        _mockRepo.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnHunter_WhenExists()
    {
        // Arrange
        int hunterId = 1;
        var hunter = new HunterDTO { Id_Hunter = hunterId, Name = "Gon", Age = 12, Origin = "Whale Island" };
        _mockRepo.Setup(r => r.GetByIdAsync(hunterId)).ReturnsAsync(hunter);

        // Act
        var result = await _hunterService.GetByIdAsync(hunterId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(hunterId, result.Id_Hunter);
        _mockRepo.Verify(r => r.GetByIdAsync(hunterId), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotExists()
    {
        // Arrange
        int hunterId = 2;
        _mockRepo.Setup(r => r.GetByIdAsync(hunterId)).ReturnsAsync((HunterDTO?)null);

        // Act
        var result = await _hunterService.GetByIdAsync(hunterId);
        
        // Assert
        Assert.Null(result);
        _mockRepo.Verify(r => r.GetByIdAsync(hunterId), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnTrue_WhenUpdateSucceeds()
    {
        // Arrange
        var hunter = new HunterDTO { Id_Hunter = 1, Name = "Gon", Age = 12, Origin = "Whale Island" };
        _mockRepo.Setup(r => r.UpdateAsync(hunter)).ReturnsAsync(true);

        // Act
        var result = await _hunterService.UpdateAsync(hunter);
        
        // Assert
        Assert.True(result);
        _mockRepo.Verify(r => r.UpdateAsync(hunter), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnFalse_WhenUpdateFails()
    {
        // Arrange
        var hunter = new HunterDTO { Id_Hunter = 2, Name = "Killua", Age = 12, Origin = "Zoldyck" };
        _mockRepo.Setup(r => r.UpdateAsync(hunter)).ReturnsAsync(false);

        // Act
        var result = await _hunterService.UpdateAsync(hunter);

        // Assert
        Assert.False(result);
        _mockRepo.Verify(r => r.UpdateAsync(hunter), Times.Once);
    }
}
