using API.Application.Interfaces;
using API.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Presentation;

[ApiController]
[Route("api/[controller]")]
public class GasController : ControllerBase
{
    private readonly IGasService _gasService;

    public GasController(IGasService gasService)
    {
        _gasService = gasService;
    }

    [HttpPost]
    public async Task<ActionResult<RefuelPriceDto>> CalculateGasPricing([FromBody]RefuelDto refuelDto)
    {
        return await _gasService.CalculateGasPrice(refuelDto);
    }
    
    [Authorize]
    [HttpPost("/secure")]
    public async Task<ActionResult<RefuelPriceDto>> CalculateGasPricingSecure([FromBody]RefuelDto refuelDto)
    {
        return await _gasService.CalculateGasPrice(refuelDto);
    }
}