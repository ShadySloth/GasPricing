using System.Runtime;
using API.Application.Interfaces;
using API.Application.Services;
using API.Domain.DTOs;
using Reqnroll;
using Tests.Util.Fixtures;

namespace Tests.Specs.StepDefinitions;

[Binding]
public sealed class CalculateGasPriceStepDefinitions
{
    private readonly GasFixture _gasFixture;
    private IGasService _gasService;
    private RefuelDto _refuelDto;
    private RefuelPriceDto _result;

    public CalculateGasPriceStepDefinitions(GasFixture gasFixture)
    {
        _gasFixture = gasFixture;
        _gasService = new GasService(_gasFixture.GasTypeRepositoryMock.Object);
    }

    [Given("you refuel {int} liters of {string} gas")]
    public void GivenYouRefuelLitersOfGas(int amount, string gasType)
    {
        _refuelDto = new RefuelDto()
        {
            Liters = amount,
            GasTypeName = gasType,
            Membership = false
        };
    }

    [Given("the user has membership status set to {Boolean}")]
    public void GivenTheUserHasMembershipStatusSetToTrue(bool membership)
    {
        _refuelDto.Membership = membership;
    }

    [When("the application calculates the gas price")]
    public async Task WhenTheApplicationCalculatesTheGasPrice()
    {
        _result = await _gasService.CalculateGasPrice(_refuelDto);
    }

    [Then("the total price should be {double}")]
    public void ThenTheTotalPriceShouldBe(double totalPrice)
    {
        Assert.Equal(totalPrice, _result.TotalPrice);
    }
}