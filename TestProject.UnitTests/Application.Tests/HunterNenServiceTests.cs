using ClassLibrary.Application.Services;
using ClassLibrary.Core.Common;
using ClassLibrary.Core.DTOs;
using ClassLibrary.Core.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestProject.UnitTests.Application.Tests;

public class HunterNenServiceTests
{
    private readonly Mock<ILogger<HunterService>> _mockLogger;
    private readonly Mock<IHunterNenRepository> _mockRepo;
    private readonly HunterNenService _hunterNenService;

    public HunterNenServiceTests()
    {
        _mockLogger = new Mock<ILogger<HunterService>>();
        _mockRepo = new Mock<IHunterNenRepository>();
        _hunterNenService = new HunterNenService(_mockLogger.Object, _mockRepo.Object);
    }

    [Fact]
    public async Task GetAllHunterNensAsync_ShouldReturnListOfHunterNens()
    {
        // Arrange
        var hunterNens = new List<HunterNenDTO>
        {
            new HunterNenDTO { Id_Hunter = 1, Id_NenType = 1, NenLevel = 50 },
            new HunterNenDTO { Id_Hunter  = 2, Id_NenType = 2, NenLevel = 80 }
        };
        _mockRepo.Setup(r => r.GetAllHunterNensAsync()).ReturnsAsync(hunterNens);
       
        // Act
        var result = await _hunterNenService.GetAllHunterNensAsync();
        
        // Assert
        Assert.Equal(hunterNens.Count, result.Count());
        _mockRepo.Verify(r => r.GetAllHunterNensAsync(), Times.Once);
    }

    [Fact]
    public async Task GetHunterNenByIdAsync_ShouldReturnHunterNen_WhenExists()
    {
        // Arrange
        var hunterNen = new HunterNenDTO { Id_Hunter = 1, Id_NenType = 1, NenLevel = 50 };
        _mockRepo.Setup(r => r.GetHunterNenByIdAsync(hunterNen)).ReturnsAsync(hunterNen);

        // Act
        var result = await _hunterNenService.GetHunterNenByIdAsync(hunterNen);

        // Assert
        Assert.Equal(hunterNen, result);
        _mockRepo.Verify(r => r.GetHunterNenByIdAsync(hunterNen), Times.Once);
    }

    [Fact]
    public async Task GetHunterNenByIdAsync_ShouldReturnNull_WhenNotExists()
    {
        // Arrange
        var hunterNen = new HunterNenDTO { Id_Hunter = 1, Id_NenType = 1, NenLevel = 50 };
        _mockRepo.Setup(r => r.GetHunterNenByIdAsync(hunterNen)).ReturnsAsync((HunterNenDTO?)null);

        // Act
        var result = await _hunterNenService.GetHunterNenByIdAsync(hunterNen);
        
        // Assert
        Assert.Null(result);
        _mockRepo.Verify(r => r.GetHunterNenByIdAsync(hunterNen), Times.Once);
    }

    [Fact]
    public async Task InsertHunterNenAsync_ShouldReturnTrue_WhenInsertionSucceeds()
    {
        // Arrange
        var hunterNen = new HunterNenDTO { Id_Hunter = 1, Id_NenType = 1, NenLevel = 50 };
        _mockRepo.Setup(r => r.InsertHunterNenAsync(hunterNen)).ReturnsAsync(Result<bool>.Success(true));

        // Act
        var result = await _hunterNenService.InsertHunterNenAsync(hunterNen);
        
        // Assert
        Assert.True(result.IsSuccess);
        _mockRepo.Verify(r => r.InsertHunterNenAsync(hunterNen), Times.Once);
    }

    [Fact]
    public async Task InsertHunterNenAsync_ShouldReturnFalse_WhenInsertionFails()
    {
        // Arrange
        var hunterNen = new HunterNenDTO { Id_Hunter = 1, Id_NenType = 1, NenLevel = 50 };
        _mockRepo.Setup(r => r.InsertHunterNenAsync(hunterNen)).ReturnsAsync(Result<bool>.Failure("Insertion failed"));
    
        // Act
        var result = await _hunterNenService.InsertHunterNenAsync(hunterNen);
        
        // Assert
        Assert.False(result.IsSuccess);
        _mockRepo.Verify(r => r.InsertHunterNenAsync(hunterNen), Times.Once);
    }

    [Fact]
    public async Task UpdateHunterNenAsync_ShouldReturnTrue_WhenUpdateSucceeds()
    {
        // Arrange
        var hunterNen = new HunterNenDTO { Id_Hunter = 1, Id_NenType = 1, NenLevel = 50 };
        _mockRepo.Setup(r => r.UpdateHunterNenAsync(hunterNen)).ReturnsAsync(Result<bool>.Success(true));

        // Act
        var result = await _hunterNenService.UpdateHunterNenAsync(hunterNen);

        // Assert
        Assert.True(result.IsSuccess);
        _mockRepo.Verify(r => r.UpdateHunterNenAsync(hunterNen), Times.Once);
    }

    [Fact]
    public async Task UpdateHunterNenAsync_ShouldReturnFalse_WhenUpdateFails()
    {
        // Arrange
        var hunterNen = new HunterNenDTO { Id_Hunter = 1, Id_NenType = 1, NenLevel = 50 };
        _mockRepo.Setup(r => r.UpdateHunterNenAsync(hunterNen)).ReturnsAsync(Result<bool>.Failure("Update failed"));

        // Act
        var result = await _hunterNenService.UpdateHunterNenAsync(hunterNen);

        // Assert
        Assert.False(result.IsSuccess);
        _mockRepo.Verify(r => r.UpdateHunterNenAsync(hunterNen), Times.Once);
    }

    [Fact]
    public async Task DeleteHunterNenAsync_ShouldReturnTrue_WhenDeletionSucceeds()
    {
        // Arrange
        var hunterNen = new HunterNenDTO { Id_Hunter = 1, Id_NenType = 1, NenLevel = 50 };
        _mockRepo.Setup(r => r.DeleteHunterNenAsync(hunterNen)).ReturnsAsync(Result<bool>.Success(true));

        // Act
        var result = await _hunterNenService.DeleteHunterNenAsync(hunterNen);
        
        // Assert
        Assert.True(result.IsSuccess);
        _mockRepo.Verify(r => r.DeleteHunterNenAsync(hunterNen), Times.Once);
    }

    [Fact]
    public async Task DeleteHunterNenAsync_ShouldReturnFalse_WhenDeletionFails()
    {
        // Arrange
        var hunterNen = new HunterNenDTO { Id_Hunter = 1, Id_NenType = 1, NenLevel = 50 };
        _mockRepo.Setup(r => r.DeleteHunterNenAsync(hunterNen)).ReturnsAsync(Result<bool>.Failure("Deletion failed"));
       
        // Act
        var result = await _hunterNenService.DeleteHunterNenAsync(hunterNen);

        // Assert
        Assert.False(result.IsSuccess);
        _mockRepo.Verify(r => r.DeleteHunterNenAsync(hunterNen), Times.Once);
    }
}
