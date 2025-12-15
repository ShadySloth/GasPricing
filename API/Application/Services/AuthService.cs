using System.IdentityModel.Tokens.Jwt;
using API.Application.Interfaces;
using API.Domain.Entities;
using API.Infrastructure.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace API.Application.Services;

using System.Security.Claims;
using System.Text;
public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepo;
    private readonly IConfiguration _config;

    public AuthService(IUserRepository userRepo, IConfiguration config)
    {
        _userRepo = userRepo;
        _config = config;


    }

    public async Task RegisterAsync(string userName, string password)
    {
        var salt = BCrypt.Net.BCrypt.GenerateSalt();
        var hash = BCrypt.Net.BCrypt.HashPassword(password, salt);

        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = userName,
            PasswordHash = new PasswordHash
            {
                Id = Guid.NewGuid(),
                Hash = hash,
                Salt = new PasswordSalt
                {
                    Id = Guid.NewGuid(),
                    Salt = salt
                }
            }
        };

        await _userRepo.CreateUserAsync(user);
    }

    public async Task<string> LoginAsync(string userName, string password)
    {
        var user = await _userRepo.GetByUserNameAsync(userName);
        if (user == null)
            throw new UnauthorizedAccessException();

        var isValid = BCrypt.Net.BCrypt.Verify(
            password,
            user.PasswordHash.Hash
        );

        if (!isValid)
            throw new UnauthorizedAccessException();

        return GenerateJwt(user);
    }

    private string GenerateJwt(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)
        );

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(
                int.Parse(_config["Jwt:ExpiresMinutes"]!)
            ),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
