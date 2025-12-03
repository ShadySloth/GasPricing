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

        var discountPercentage = 1.00;
        if (refuelDto.Membership)
        {
            discountPercentage = 0.90;
        }

        discountPercentage = refuelDto.Liters switch
        {
            <= 20 => discountPercentage,
            <= 50 => discountPercentage - 0.05,
            <= 100 => discountPercentage - 0.10,
            > 100 => discountPercentage - 0.15
        };

        var totalPrice = (refuelDto.Liters * pricePerLiter) * discountPercentage;
        
        return new RefuelPriceDto
        {
            TotalPrice = totalPrice
        };
    }

    private async Task<double> GetGasPrice(string gasTypeName)
    {
        if (string.IsNullOrEmpty(gasTypeName))
        {
            throw new ArgumentNullException(nameof(gasTypeName),"Gas type name cannot be null or empty");
        }
        
        var gasType = await _gasTypeRepository.GetGasTypeByName(gasTypeName);
        
        if (gasType == null)
        {
            throw new ArgumentException("Invalid gas type.");
        }

        return gasType.Price;
    }
}