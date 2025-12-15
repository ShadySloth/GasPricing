using API.Application.Interfaces;

namespace API.Presentation;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(string userName, string password)
    {
        await _authService.RegisterAsync(userName, password);
        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(string userName, string password)
    {
        var token = await _authService.LoginAsync(userName, password);
        return Ok(new { token });
    }
}
