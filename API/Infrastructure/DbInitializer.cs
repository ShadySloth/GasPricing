using API.Domain.Entities;
using API.Infrastructure.Contexts;
using API.Infrastructure.Interfaces;

namespace API.Infrastructure;

public class DbInitializer : IDbInitializer
{
    public void InitializeDev(GasContext context)
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
    
    /**
     * Check if db is there with the needed data in tables, if not create them
     */
    public void InitializeProd(GasContext context)
    {
        context.Database.EnsureCreated();
        
        List<GasType> gasTypes = new()
        {
            new GasType { Id = 1, Name = "Regular", Price = 12.70 },
            new GasType { Id = 2, Name = "Premium", Price = 14.50 },
            new GasType { Id = 3, Name = "Diesel", Price = 7.20 }
        };
        foreach (var gasType in gasTypes)
        {
            if (!context.GasTypes.Any(gt => gt.Name == gasType.Name))
            {
                context.GasTypes.Add(gasType);
            }
        }
        context.SaveChanges();
    }
}