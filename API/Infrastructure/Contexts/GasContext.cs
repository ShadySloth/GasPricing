using API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Infrastructure.Contexts;

public class GasContext : DbContext
{
    public GasContext(DbContextOptions<GasContext> options) : base(options)
    {
    }
    
    public DbSet<GasType> GasTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}