using API.Application.Interfaces;
using API.Domain.DTOs;
using API.Domain.Entities;
using API.Infrastructure.Interfaces;

namespace API.Application.Services;

public class GasService : IGasService
{
    private readonly IGasTypeRepository _gasTypeRepository;

    public GasService(IGasTypeRepository gasTypeRepository)
    {
        _gasTypeRepository = gasTypeRepository;
    }

    public async Task<RefuelPriceDto> CalculateGasPrice(RefuelDto refuelDto)
    {
        if (refuelDto.Liters <= 0)
            throw new ArgumentOutOfRangeException("Liters", "Liters must be greater than zero");

        var pricePerLiter = await GetGasPrice(refuelDto.GasTypeName);

        var discountPrecentage = 1.00;
        if (refuelDto.Membership)
        {
            discountPrecentage = 0.90;
        }

        var totalPrice = refuelDto.Liters switch
        {
            <= 20 => pricePerLiter * refuelDto.Liters,
            <= 50 => pricePerLiter * refuelDto.Liters + pricePerLiter * (discountPrecentage - 0.05) * (refuelDto.Liters - 20),
            <= 100 => pricePerLiter * refuelDto.Liters + pricePerLiter * (discountPrecentage - 0.05) * (refuelDto.Liters - 30),
            > 100 => pricePerLiter * refuelDto.Liters + pricePerLiter * (discountPrecentage - 0.05) * (refuelDto.Liters - 50)
        };

        return new RefuelPriceDto
        {
            TotalPrice = totalPrice
        };
    }

    private async Task<double> GetGasPrice(string gasTypeName)
    {
        var gasType = await _gasTypeRepository.GetGasTypeByName(gasTypeName);

        if (gasType == null)
            throw new ArgumentException("Invalid gas type");

        return gasType.Price;
    }
}