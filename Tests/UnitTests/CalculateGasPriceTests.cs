using API.Application.Services;
using API.Domain.DTOs;
using Tests.Util;
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
    [MemberData(nameof(TestDataGenerator.GetValidData), MemberType = typeof(TestDataGenerator))]
    public async Task CalculateGasPrice_ReturnsTotalPrice_Correctly(
        string gasTypeName, 
        int liters, 
        bool membership, 
        decimal expectedPrice
        )
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
        var result = await gasService.CalculateGasPrice(refuelDto);
        
        // Assert
        Assert.Equal(expectedPrice, result.TotalPrice);
    }
    
    [Theory]
    [MemberData(nameof(TestDataGenerator.GetDataWithWrongLiters), MemberType = typeof(TestDataGenerator))]
    public async Task CalculateGasPrice_ForInvalidLiters_ThrowsArgumentOutOfRangeException(
        string gasTypeName, 
        int liters, 
        bool membership
        )
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
    
    [Theory]
    [MemberData(nameof(TestDataGenerator.GetDataWithWrongGasType), MemberType = typeof(TestDataGenerator))]
    public async Task CalculateGasPrice_ForInvalidGasType_ThrowsArgumentException(
        string gasTypeName, 
        int liters, 
        bool membership
        )
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
        await Assert.ThrowsAsync<ArgumentException>(result);
    }
    
    [Theory]
    [MemberData(nameof(TestDataGenerator.GetDataWithNullGasType), MemberType = typeof(TestDataGenerator))]
    public async Task CalculateGasPrice_ForNullGasTypeName_ThrowsArgumentNullException(
        string gasTypeName, 
        int liters, 
        bool membership
        )
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
        await Assert.ThrowsAsync<ArgumentNullException>(result);
    }

    #endregion
}