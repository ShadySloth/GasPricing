using API.Infrastructure.Contexts;

namespace API.Infrastructure.Interfaces;

public interface IDbInitializer
{
    void InitializeDev(GasContext context);
    void InitializeProd(GasContext context);
}