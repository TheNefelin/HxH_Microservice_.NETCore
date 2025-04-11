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
    public async Task InsertHunterNenAsync_AlreadyExists_ReturnsFailure()
    {
        // Arrange
        var existingDto = new HunterNenDTO { Id_Hunter = 1, Id_NenType = 1, NenLevel = 50 };

        // Act
        var result = await _hunterNenRepository.InsertHunterNenAsync(existingDto);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("HunterNen already exists.", result.Message);
    }

    [Fact]
    public async Task InsertHunterNenAsync_ValidDto_InsertsSuccessfullyAndCleansUp()
    {
        // Arrange
        var dto = new HunterNenDTO { Id_Hunter = 4, Id_NenType = 4, NenLevel = 50 };

        // Act
        var result = await _hunterNenRepository.InsertHunterNenAsync(dto);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(result.Data);
        Assert.Equal("Inserted successfully.", result.Message);

        // Clean up: borrar el registro insertado
        await _hunterNenRepository.DeleteHunterNenAsync(dto);
    }

    [Fact]
    public async Task InsertHunterNenAsync_HunterNotFound_ReturnsFailure()
    {
        // Arrange
        var dto = new HunterNenDTO { Id_Hunter = 9999, Id_NenType = 1, NenLevel = 50 };

        // Act
        var result = await _hunterNenRepository.InsertHunterNenAsync(dto);

        // Assert
        Assert.False(result.IsSuccess);
        //Assert.Equal("Hunter not found.", result.Message);
    }

    [Fact]
    public async Task InsertHunterNenAsync_NenTypeNotFound_ReturnsFailure()
    {
        // Arrange
        var dto = new HunterNenDTO { Id_Hunter = 1, Id_NenType = 9999, NenLevel = 50 };

        // Act
        var result = await _hunterNenRepository.InsertHunterNenAsync(dto);

        // Assert
        Assert.False(result.IsSuccess);
        //Assert.Equal("NenType not found.", result.Message);
    }

}
