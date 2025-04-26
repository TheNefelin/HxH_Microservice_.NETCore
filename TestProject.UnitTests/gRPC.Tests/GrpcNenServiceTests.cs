using ClassLibrary.Core.DTOs;
using ClassLibrary.Core.Interfaces;
using gRPC.NenService.Protos;
using gRPC.NenService.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestProject.UnitTests.gRPC.Tests;

public class GrpcNenServiceTests
{
    private readonly Mock<INenService> _mockService;
    private readonly NenGrpService _grpcService;

    public GrpcNenServiceTests()
    {
        _mockService = new Mock<INenService>();
        var logger = Mock.Of<ILogger<NenGrpService>>();
        _grpcService = new NenGrpService(logger, _mockService.Object);
    }

    [Fact]
    public async Task NenGetAll_ReturnsCorrectList()
    {
        // Arrange
        var mockData = new List<NenTypeDTO>
        {
            new NenTypeDTO { Id_NenType = 1, Name = "Intensification", Description = "Intensification" },
            new NenTypeDTO { Id_NenType = 2, Name = "Manipulation", Description = "Manipulation" }
        };
        _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(mockData);

        // Act
        var result = await _grpcService.GetAllNenTypes(new Empty(), TestServerCallContext.Create());

        // Assert
        Assert.Equal(2, result.NenTypes.Count);
        Assert.Equal(1, result.NenTypes[0].Id);
        Assert.Equal("Intensification", result.NenTypes[0].Name);
        Assert.Equal("Manipulation", result.NenTypes[1].Description);
    }
}