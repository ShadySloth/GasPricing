using API.Domain.Entities;
using API.Infrastructure.Interfaces;
using Moq;

namespace Tests.Util.Fixtures;

public class GasFixture
{
    public Mock<IGasTypeRepository> GasTypeRepositoryMock { get; }

    public GasFixture()
    {
        GasTypeRepositoryMock = new Mock<IGasTypeRepository>(MockBehavior.Strict);
        
        GasTypeRepositoryMock.Setup(repo => repo.GetGasTypeByName("Regular"))
            .ReturnsAsync(new GasType { Id = 1, Name = "Regular", Price = 12.70 });
        
        GasTypeRepositoryMock.Setup(repo => repo.GetGasTypeByName("Premium"))
            .ReturnsAsync(new GasType { Id = 2, Name = "Premium", Price = 14.50 });
        
        GasTypeRepositoryMock.Setup(repo => repo.GetGasTypeByName("Diesel"))
            .ReturnsAsync(new GasType { Id = 3, Name = "Diesel", Price = 7.20 });
        
        GasTypeRepositoryMock.Setup(repo => repo.GetGasTypeByName(It.Is<string>(n =>
                n != "Regular" && n != "Premium" && n != "Diesel")))
            .ReturnsAsync((GasType?)null);
    }
}