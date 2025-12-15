using API.Domain.Entities;

namespace API.Infrastructure.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByUserNameAsync(string userName);
    Task CreateUserAsync(User user);
}