namespace API.Application.Interfaces;

public interface IAuthService
{
    Task RegisterAsync(string userName, string password);
    Task<string> LoginAsync(string userName, string password);
}