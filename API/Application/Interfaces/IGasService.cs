using API.Domain.DTOs;
using API.Domain.Entities;

namespace API.Application.Interfaces;

public interface IGasService
{
    Task<RefuelPriceDto> CalculateGasPrice(RefuelDto refuelDto);
}