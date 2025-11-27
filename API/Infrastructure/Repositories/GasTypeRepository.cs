using API.Domain.Entities;
using API.Infrastructure.Contexts;
using API.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Infrastructure.Repositories;

public class GasTypeRepository : IGasTypeRepository
{
    private readonly GasContext _context;
    public GasTypeRepository(GasContext context)
    {
        _context = context;
    }
    
    public async Task<GasType> GetGasTypeByName(string name)
    {
        var gasType = await _context.GasTypes
            .FirstOrDefaultAsync(gt => gt.Name == name);
        return gasType;
    }
}