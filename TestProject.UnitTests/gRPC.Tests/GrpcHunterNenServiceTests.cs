using ClassLibrary.Core.Common;
using ClassLibrary.Core.DTOs;
using ClassLibrary.Core.Interfaces;
using gRPC.HunterNenService;
using gRPC.HunterNenService.Services;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestProject.UnitTests.gRPC.Tests;

public class GrpcHunterNenServiceTests
{
    private readonly Mock<IHunterNenService> _mockService;
    private readonly GrpcHunterNenService _grpcService;

    public GrpcHunterNenServiceTests()
    {
        _mockService = new Mock<IHunterNenService>();
        var logger = Mock.Of<ILogger<GrpcHunterNenService>>();
        _grpcService = new GrpcHunterNenService(logger, _mockService.Object);
    }

    [Fact]
    public async Task HunterNenGetAll_ReturnsCorrectList()
    {
        // Arrange
        var mockData = new List<HunterNenDTO>
        {
            new HunterNenDTO { Id_Hunter = 1, Id_NenType = 2, NenLevel = 4.5f },
            new HunterNenDTO { Id_Hunter = 2, Id_NenType = 3, NenLevel = 3.2f }
        };
        _mockService.Setup(s => s.GetAllHunterNensAsync()).ReturnsAsync(mockData);

        // Act
        var result = await _grpcService.HunterNenGetAll(new Empty(), TestServerCallContext.Create());

        // Assert
        Assert.Equal(2, result.HunterNens.Count);
        Assert.Equal(1, result.HunterNens[0].IdHunter);
        Assert.Equal(3, result.HunterNens[1].IdNen);
    }

    [Fact]
    public async Task HunterNenGetById_ReturnsCorrectItem()
    {
        // Arrange
        var mockData = new HunterNenDTO { Id_Hunter = 1, Id_NenType = 2, NenLevel = 4.5f };
        var request = new HunterNenRequest { IdHunter = 1, IdNen = 2 };
        _mockService.Setup(s => s.GetHunterNenByIdAsync(It.IsAny<HunterNenDTO>())).ReturnsAsync(mockData);
        
        // Act
        var result = await _grpcService.HunterNenGetById(request, TestServerCallContext.Create());
        
        // Assert
        Assert.Equal(1, result.IdHunter);
        Assert.Equal(2, result.IdNen);
    }

    [Fact]
    public async Task HunterNenGetById_ReturnsNotFound()
    {
        // Arrange
        var request = new HunterNenRequest { IdHunter = 1, IdNen = 2 };
        _mockService.Setup(s => s.GetHunterNenByIdAsync(It.IsAny<HunterNenDTO>())).ReturnsAsync((HunterNenDTO?)null);

        // Act & Assert
        await Assert.ThrowsAsync<RpcException>(() => _grpcService.HunterNenGetById(request, TestServerCallContext.Create()));
    }

    [Fact]
    public async Task HunterNenInsert_ReturnsSuccess()
    {
        // Arrange
        var request = new HunterNenRequest { IdHunter = 1, IdNen = 2, NenLevel = 4.5f };
        var dto = new HunterNenDTO { Id_Hunter = 1, Id_NenType = 2, NenLevel = 4.5f };
        _mockService.Setup(s => s.InsertHunterNenAsync(It.IsAny<HunterNenDTO>())).ReturnsAsync(Result<bool>.Success(true));
       
        // Act
        var result = await _grpcService.HunterNenInsert(request, TestServerCallContext.Create());
        
        // Assert
        Assert.True(result.Success);
    }

    [Fact]
    public async Task HunterNenInsert_ReturnsFailure()
    {
        // Arrange
        var request = new HunterNenRequest { IdHunter = 1, IdNen = 2, NenLevel = 4.5f };
        var dto = new HunterNenDTO { Id_Hunter = 1, Id_NenType = 2, NenLevel = 4.5f };
        _mockService.Setup(s => s.InsertHunterNenAsync(It.IsAny<HunterNenDTO>())).ReturnsAsync(Result<bool>.Failure("Insert failed"));

        // Act
        var result = await _grpcService.HunterNenInsert(request, TestServerCallContext.Create());

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Insert failed", result.Message);
    }

    [Fact]
    public async Task HunterNenUpdate_ReturnsSuccess()
    {
        // Arrange
        var request = new HunterNenRequest { IdHunter = 1, IdNen = 2, NenLevel = 4.5f };
        var dto = new HunterNenDTO { Id_Hunter = 1, Id_NenType = 2, NenLevel = 4.5f };
        _mockService.Setup(s => s.UpdateHunterNenAsync(It.IsAny<HunterNenDTO>())).ReturnsAsync(Result<bool>.Success(true));
     
        // Act
        var result = await _grpcService.HunterNenUpdate(request, TestServerCallContext.Create());
        
        // Assert
        Assert.True(result.Success);
    }

    [Fact]
    public async Task HunterNenUpdate_ReturnsFailure()
    {
        // Arrange
        var request = new HunterNenRequest { IdHunter = 1, IdNen = 2, NenLevel = 4.5f };
        var dto = new HunterNenDTO { Id_Hunter = 1, Id_NenType = 2, NenLevel = 4.5f };
        _mockService.Setup(s => s.UpdateHunterNenAsync(It.IsAny<HunterNenDTO>())).ReturnsAsync(Result<bool>.Failure("Update failed"));
       
        // Act
        var result = await _grpcService.HunterNenUpdate(request, TestServerCallContext.Create());
        
        // Assert
        Assert.False(result.Success);
        Assert.Equal("Update failed", result.Message);
    }

    [Fact]
    public async Task HunterNenDelete_ReturnsSuccess()
    {
        // Arrange
        var request = new HunterNenRequest { IdHunter = 1, IdNen = 2 };
        var dto = new HunterNenDTO { Id_Hunter = 1, Id_NenType = 2 };
        _mockService.Setup(s => s.DeleteHunterNenAsync(It.IsAny<HunterNenDTO>())).ReturnsAsync(Result<bool>.Success(true));

        // Act
        var result = await _grpcService.HunterNenDelete(request, TestServerCallContext.Create());

        // Assert
        Assert.True(result.Success);
    }

    [Fact]
    public async Task HunterNenDelete_ReturnsFailure()
    {
        // Arrange
        var request = new HunterNenRequest { IdHunter = 1, IdNen = 2 };
        var dto = new HunterNenDTO { Id_Hunter = 1, Id_NenType = 2 };
        _mockService.Setup(s => s.DeleteHunterNenAsync(It.IsAny<HunterNenDTO>())).ReturnsAsync(Result<bool>.Failure("Delete failed"));
    
        // Act
        var result = await _grpcService.HunterNenDelete(request, TestServerCallContext.Create());
        
        // Assert
        Assert.False(result.Success);
        Assert.Equal("Delete failed", result.Message);
    }
}

