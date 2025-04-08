using ClassLibrary.Core.DTOs;
using ClassLibrary.Infrastructure.Repositories;
using Microsoft.Extensions.Logging.Abstractions;

namespace TestProject.UnitTests.Infrastructure.Tests;

[Collection("Database collection")]
public class NenRepositoryIntegrationTests
{
    private readonly NenRepository _nenRepository;

    public NenRepositoryIntegrationTests(DatabaseFixture fixture)
    {
        var logger = NullLogger<NenRepository>.Instance;
        _nenRepository = new NenRepository(fixture.DbContext, logger);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllNenTypes()
    {
        // Act
        var result = await _nenRepository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnCorrectNenType()
    {
        // Arrange
        var nenType = new NenTypeDTO { Name = "Transmutation", Description = "Transformación de la materia" };
        var idNenType = await _nenRepository.CreateAsync(nenType);

        // Act
        var result = await _nenRepository.GetByIdAsync(idNenType);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(nenType.Name, result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNenTypeDoesNotExist()
    {
        // Arrange
        var idNenType = 9999; // Assuming this ID does not exist in the database
     
        // Act
        var result = await _nenRepository.GetByIdAsync(idNenType);
        
        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_ShouldInsertNenType()
    {
        // Arrange
        var nenType = new NenTypeDTO { Name = "Transmutation", Description = "Transformación de la materia" };

        // Act
        var idNenType = await _nenRepository.CreateAsync(nenType);

        // Assert
        Assert.True(idNenType > 0);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowArgumentNullException_WhenNenTypeIsNull()
    {
        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _nenRepository.CreateAsync(null));
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveNenType()
    {
        // Arrange
        var nenType = new NenTypeDTO { Name = "Transmutation", Description = "Transformación de la materia" };
        var idNenType = await _nenRepository.CreateAsync(nenType);
        
        // Act
        var result = await _nenRepository.DeleteAsync(idNenType);
     
        // Assert
        Assert.True(result);
    }

    [Fact] 
    public async Task DeleteAsync_ShouldReturnFalse_WhenNenTypeDoesNotExist()
    {
        // Arrange
        var idNenType = 9999; // Assuming this ID does not exist in the database

        // Act
        var result = await _nenRepository.DeleteAsync(idNenType);

        // Assert
        Assert.False(result);
    }
}
