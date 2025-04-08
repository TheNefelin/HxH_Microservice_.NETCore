using Castle.Core.Logging;
using ClassLibrary.Application.Services;
using ClassLibrary.Core.DTOs;
using ClassLibrary.Core.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestProject.UnitTests.Application.Tests;

public class NenServiceTests
{
    private readonly ILogger<NenService> _mockLogger;
    private readonly Mock<INenRepository> _mockRepo;
    private readonly NenService _nenService;

    public NenServiceTests()
    {
        _mockRepo = new Mock<INenRepository>();
        _mockLogger = new Mock<ILogger<NenService>>().Object;
        _nenService = new NenService(_mockLogger, _mockRepo.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnTrue_WhenCreationSucceeds()
    {
        // Arrange
        var nenType = new NenTypeDTO { Id_NenType = 1, Name = "Enhancement" };
        _mockRepo.Setup(r => r.CreateAsync(nenType)).ReturnsAsync(nenType.Id_NenType);
     
        // Act
        var idNenType = await _nenService.CreateAsync(nenType);
        
        // Assert
        Assert.Equal(idNenType, nenType.Id_NenType);
        _mockRepo.Verify(r => r.CreateAsync(nenType), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnFalse_WhenCreationFails()
    {
        // Arrange
        var nenType = new NenTypeDTO { Id_NenType = 2, Name = "Transmutation" };
        _mockRepo.Setup(r => r.CreateAsync(nenType)).ReturnsAsync(nenType.Id_NenType + 1);

        // Act
        var idNenType = await _nenService.CreateAsync(nenType);

        // Assert
        Assert.NotEqual(nenType.Id_NenType, idNenType);
        _mockRepo.Verify(r => r.CreateAsync(nenType), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnTrue_WhenDeletionSucceeds()
    {
        // Arrange
        int nenTypeId = 1;
        _mockRepo.Setup(r => r.DeleteAsync(nenTypeId)).ReturnsAsync(true);
        // Act
        var result = await _nenService.DeleteAsync(nenTypeId);
        // Assert
        Assert.True(result);
        _mockRepo.Verify(r => r.DeleteAsync(nenTypeId), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_WhenDeletionFails()
    {
        // Arrange
        int nenTypeId = 2;
        _mockRepo.Setup(r => r.DeleteAsync(nenTypeId)).ReturnsAsync(false);
        // Act
        var result = await _nenService.DeleteAsync(nenTypeId);
        // Assert
        Assert.False(result);
        _mockRepo.Verify(r => r.DeleteAsync(nenTypeId), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnNenTypes_WhenNenTypesExist()
    {
        // Arrange
        var nenTypes = new List<NenTypeDTO>
        {
            new NenTypeDTO { Id_NenType = 1, Name = "Enhancement" },
            new NenTypeDTO { Id_NenType = 2, Name = "Transmutation" }
        };
        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(nenTypes);

        // Act
        var result = await _nenService.GetAllAsync();

        // Assert
        Assert.Equal(nenTypes.Count, result.Count());
        _mockRepo.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnEmptyList_WhenNenTypesDoNotExist()
    {
        // Arrange
        var nenTypes = new List<NenTypeDTO>();
        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(nenTypes);

        // Act
        var result = await _nenService.GetAllAsync();
        // Assert

        Assert.Empty(result);
        _mockRepo.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNenType_WhenNenTypeExists()
    {
        // Arrange
        int nenTypeId = 1;
        var nenType = new NenTypeDTO { Id_NenType = nenTypeId, Name = "Enhancement" };
        _mockRepo.Setup(r => r.GetByIdAsync(nenTypeId)).ReturnsAsync(nenType);

        // Act
        var result = await _nenService.GetByIdAsync(nenTypeId);
        
        // Assert
        Assert.Equal(nenType, result);
        _mockRepo.Verify(r => r.GetByIdAsync(nenTypeId), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNenTypeDoesNotExist()
    {
        // Arrange
        int nenTypeId = 2;
        _mockRepo.Setup(r => r.GetByIdAsync(nenTypeId)).ReturnsAsync((NenTypeDTO?)null);
        // Act
        var result = await _nenService.GetByIdAsync(nenTypeId);
        // Assert
        Assert.Null(result);
        _mockRepo.Verify(r => r.GetByIdAsync(nenTypeId), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnTrue_WhenUpdateSucceeds()
    {
        // Arrange
        var nenType = new NenTypeDTO { Id_NenType = 1, Name = "Enhancement" };
        _mockRepo.Setup(r => r.UpdateAsync(nenType)).ReturnsAsync(true);
        
        // Act
        var result = await _nenService.UpdateAsync(nenType);
        
        // Assert
        Assert.True(result);
        _mockRepo.Verify(r => r.UpdateAsync(nenType), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnFalse_WhenUpdateFails()
    {
        // Arrange
        var nenType = new NenTypeDTO { Id_NenType = 2, Name = "Transmutation" };
        _mockRepo.Setup(r => r.UpdateAsync(nenType)).ReturnsAsync(false);
        
        // Act
        var result = await _nenService.UpdateAsync(nenType);
     
        // Assert
        Assert.False(result);
        _mockRepo.Verify(r => r.UpdateAsync(nenType), Times.Once);
    }
}
