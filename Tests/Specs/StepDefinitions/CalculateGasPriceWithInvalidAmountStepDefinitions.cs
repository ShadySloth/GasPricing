using API.Application.Interfaces;
using API.Application.Services;
using API.Domain.DTOs;
using Reqnroll;
using Tests.Util.Fixtures;

namespace Tests;

[Binding]
public class CalculateGasPriceWithInvalidAmountStepDefinitions
{
    private readonly GasFixture _gasFixture;
    private IGasService _gasService;
    private RefuelDto _refuelDto;
    private Task _result;
    
    public CalculateGasPriceWithInvalidAmountStepDefinitions(GasFixture gasFixture)
    {
        _gasFixture = gasFixture;
        _gasService = new GasService(_gasFixture.GasTypeRepositoryMock.Object);
    }
    
    [Given("you refuel with {int} liters of gas")]
    public void GivenYouRefuelWithLitersOfGas(int amount)
    {
        _refuelDto = new RefuelDto()
        {
            Liters = amount,
            GasTypeName = "Regular",
            Membership = false
        };
    }

    [When("the application attempts to calculate the gas price")]
    public void WhenTheApplicationAttemptsToCalculateTheGasPrice()
    {
        _result = _gasService.CalculateGasPrice(_refuelDto);
    }

    [Then("an error message should be displayed indicating an invalid amount of gas")]
    public async Task ThenAnErrorMessageShouldBeDisplayedIndicatingAnInvalidAmountOfGas()
    {
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _result);
    }
}