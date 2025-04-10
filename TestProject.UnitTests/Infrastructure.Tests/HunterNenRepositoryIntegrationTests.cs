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
        Assert.Equal("Hunter not found.", result.Message);
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
        Assert.Equal("NenType not found.", result.Message);
    }

    //[Fact]
    //public async Task AddHunterNenAsync_ShouldReturnTrue_WhenHunterNenAlreadyExists()
    //{
    //    // Arrange
    //    var hunterNen = new HunterNenDTO
    //    {
    //        Id_Hunter = 1,
    //        Id_NenType = 1,
    //        NenLevel = 50.0f
    //    };

    //    // Act
    //    var result = await _hunterNenRepository.AddHunterNenAsync(hunterNen);

    //    // Assert
    //    Assert.True(result);
    //}

    //[Fact]
    //public async Task DeleteHunterNenAsync_ShouldDeleteHunterNen()
    //{
    //    // Arrange
    //    var hunterNen = new HunterNenDTO
    //    {
    //        Id_Hunter = 1,
    //        Id_NenType = 5,
    //        NenLevel = 50.0f
    //    };

    //    // Act
    //    var result = await _hunterNenRepository.DeleteHunterNenAsync(hunterNen);

    //    // Assert
    //    Assert.True(result);
    //}

    //[Fact]
    //public async Task DeleteHunterNenAsync_ShouldReturnTrue_WhenHunterNenDoesNotExist()
    //{
    //    // Arrange
    //    var hunterNen = new HunterNenDTO
    //    {
    //        Id_Hunter = 9999, // Assuming this ID does not exist in the database
    //        Id_NenType = 9999,
    //        NenLevel = 50.0f
    //    };

    //    // Act
    //    var result = await _hunterNenRepository.DeleteHunterNenAsync(hunterNen);

    //    // Assert
    //    Assert.True(result);
    //}

    //[Fact]
    //public async Task UpdateHunterNenAsync_ShouldReturnFalse_WhenUpdateFails()
    //{
    //    // Arrange
    //    var hunterNen = new HunterNenDTO
    //    {
    //        Id_Hunter = 9999, // Assuming this ID does not exist in the database
    //        Id_NenType = 9999,
    //        NenLevel = 50.0f
    //    };

    //    // Act
    //    var result = await _hunterNenRepository.UpdateHunterNenAsync(hunterNen);
    //    // Assert
    //    Assert.False(result);
    //}

    //[Fact]
    //public async Task UpdateHunterNenAsync_ShouldThrowArgumentNullException_WhenHunterNenIsNull()
    //{
    //    // Arrange
    //    HunterNenDTO hunterNen = null;

    //    // Act & Assert
    //    await Assert.ThrowsAsync<ArgumentNullException>(() => _hunterNenRepository.UpdateHunterNenAsync(hunterNen));
    //}

    //[Fact]
    //public async Task GetAllHunterNensAsync_ShouldReturnAllHunterNen()
    //{
    //    // Act
    //    var result = await _hunterNenRepository.GetAllHunterNensAsync();

    //    // Assert
    //    Assert.NotNull(result);
    //    Assert.NotEmpty(result);
    //}

    //[Fact]
    //public async Task GetByIdHunterNenAsync_ShouldReturnHunterNen_WhenExists()
    //{
    //    // Arrange
    //    var hunterNen = new HunterNenDTO
    //    {
    //        Id_Hunter = 1,
    //        Id_NenType = 1,
    //        NenLevel = 50.0f
    //    };

    //    // Act
    //    var result = await _hunterNenRepository.GetHunterNenByIdAsync(hunterNen);

    //    // Assert
    //    Assert.NotNull(result);
    //    Assert.Equal(hunterNen.Id_Hunter, result.Id_Hunter);
    //}

    //[Fact]
    //public async Task GetByIdHunterNenAsync_ShouldReturnNull_WhenDoesNotExist()
    //{
    //    // Arrange
    //    var hunterNen = new HunterNenDTO
    //    {
    //        Id_Hunter = 9999, // Assuming this ID does not exist in the database
    //        Id_NenType = 9999,
    //        NenLevel = 50.0f
    //    };

    //    // Act
    //    var result = await _hunterNenRepository.GetHunterNenByIdAsync(hunterNen);

    //    // Assert
    //    Assert.Null(result);
    //}
}
