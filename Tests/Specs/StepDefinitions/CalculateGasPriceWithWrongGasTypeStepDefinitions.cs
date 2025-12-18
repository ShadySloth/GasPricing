using API.Application.Interfaces;
using API.Application.Services;
using API.Domain.DTOs;
using Reqnroll;
using Tests.Util.Fixtures;

namespace Tests.Specs.StepDefinitions;

[Binding]
public class CalculateGasPriceWithWrongGasTypeStepDefinitions
{
    private readonly GasFixture _gasFixture;
    private IGasService _gasService;
    private RefuelDto _refuelDto;
    private Task _result;

    public CalculateGasPriceWithWrongGasTypeStepDefinitions(GasFixture gasFixture)
    {
        _gasFixture = gasFixture;
        _gasService = new GasService(_gasFixture.GasTypeRepositoryMock.Object);
    }

    [Given("you refuel with {string} gas")]
    public void GivenYouRefuelWithGas(string gasType)
    {
        _refuelDto = new RefuelDto()
        {
            Liters = 10,
            GasTypeName = gasType,
            Membership = false
        };
    }

    [When("the application tries to calculates the gas price")]
    public void WhenTheApplicationTriesToCalculatesTheGasPrice()
    {
        _result = _gasService.CalculateGasPrice(_refuelDto);
    }

    [Then("an error message should be displayed indicating invalid gas type")]
    public async Task ThenAnErrorMessageShouldBeDisplayedIndicatingInvalidGasType()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => _result);
    }
}