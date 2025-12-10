using API.Application.Services;
using API.Domain.DTOs;
using Tests.Util.Fixtures;

namespace Tests.UnitTests;

public class CalculateGasPriceTests : IClassFixture<GasFixture>
{
    private readonly GasFixture _fixture;
    
    public CalculateGasPriceTests(GasFixture fixture)
    {
        _fixture = fixture;
    }

    #region CalculateGasPrice Tests

    [Theory]
    [InlineData("Regular", 10, true)]
    public async Task CalculateGasPrice_ReturnsTotalPrice_Correctly(string gasTypeName, int liters, bool membership)
    {
        // Arrange
        var refuelDto = new RefuelDto
        {
            GasTypeName = gasTypeName,
            Liters = liters,
            Membership = membership
        };
        
        var gasPrice = gasTypeName switch
        {
            "Regular" => 12.70,
            "Premium" => 14.50,
            "Diesel" => 7.20,
            _ => 1
        };
        
        var discountPercentage = 1.00;
        if (membership)
        {
            discountPercentage = 0.90;
        }
        
        discountPercentage = liters switch
        {
            <= 20 => discountPercentage,
            <= 50 => discountPercentage - 0.05,
            <= 100 => discountPercentage - 0.10,
            > 100 => discountPercentage - 0.15
        };

        var expectedPrice = liters * gasPrice * discountPercentage;

        var gasService = new GasService(_fixture.GasTypeRepositoryMock.Object);
        
        // Act
        var result = await gasService.CalculateGasPrice(refuelDto);
        
        // Assert
        Assert.Equal(expectedPrice, result.TotalPrice);
    }
    
    [Theory]
    [InlineData("Regular", -10, false)]
    [InlineData("Premium", 0, true)]
    public async Task CalculateGasPrice_ForInvalidLiters_ThrowsArgumentOutOfRangeException(string gasTypeName, int liters, bool membership)
    {
        // Arrange
        var refuelDto = new RefuelDto
        {
            GasTypeName = gasTypeName,
            Liters = liters,
            Membership = membership
        };

        var gasService = new GasService(_fixture.GasTypeRepositoryMock.Object);
        
        // Act
        Task result() => gasService.CalculateGasPrice(refuelDto);
        
        // Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(result);
    }
    
    [Fact]
    public async Task CalculateGasPrice_ForInvalidGasType_ThrowsArgumentException()
    {
        // Arrange
        var refuelDto = new RefuelDto
        {
            GasTypeName = "Milk",
            Liters = 10,
            Membership = true
        };

        var gasService = new GasService(_fixture.GasTypeRepositoryMock.Object);
        
        // Act
        Task result() => gasService.CalculateGasPrice(refuelDto);
        
        // Assert
        await Assert.ThrowsAsync<ArgumentException>(result);
    }
    
    [Fact]
    public async Task CalculateGasPrice_ForNullGasTypeName_ThrowsArgumentNullException()
    {
        // Arrange
        var refuelDto = new RefuelDto
        {
            GasTypeName = null!,
            Liters = 10,
            Membership = false
        };

        var gasService = new GasService(_fixture.GasTypeRepositoryMock.Object);
        
        // Act
        Task result() => gasService.CalculateGasPrice(refuelDto);
        
        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(result);
    }

    #endregion
}