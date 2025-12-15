using API.Domain.Entities;
using API.Infrastructure.Contexts;
using API.Infrastructure.Interfaces;

namespace API.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;

public class UserRepository : IUserRepository
{
    private readonly AuthDbContext _context;

    public UserRepository(AuthDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByUserNameAsync(string userName)
    {
        return await _context.Users
            .Include(u => u.PasswordHash)
            .ThenInclude(ph => ph.Salt)
            .FirstOrDefaultAsync(u => u.UserName == userName);
    }

    public async Task CreateUserAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }
}
