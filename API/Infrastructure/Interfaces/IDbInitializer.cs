using API.Infrastructure.Contexts;

namespace API.Infrastructure.Interfaces;

public interface IDbInitializer
{
    void Initialize(GasContext context);
}