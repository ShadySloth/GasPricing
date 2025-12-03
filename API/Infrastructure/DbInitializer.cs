using API.Domain.Entities;
using API.Infrastructure.Contexts;
using API.Infrastructure.Interfaces;

namespace API.Infrastructure;

public class DbInitializer : IDbInitializer
{
    public void Initialize(GasContext context)
    {
        context.Database.EnsureDeleted();
        
        context.Database.EnsureCreated();

        List<GasType> gasTypes = new()
        {
            new GasType { Id = 1, Name = "Regular", Price = 12.70 },
            new GasType { Id = 2, Name = "Premium", Price = 14.50 },
            new GasType { Id = 3, Name = "Diesel", Price = 7.20 }
        };
        
        context.GasTypes.AddRange(gasTypes);
        context.SaveChanges();
    }
}