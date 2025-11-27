using API.Domain.Entities;

namespace API.Infrastructure.Interfaces;

public interface IGasTypeRepository
{
    Task<GasType> GetGasTypeByName(string name);
}