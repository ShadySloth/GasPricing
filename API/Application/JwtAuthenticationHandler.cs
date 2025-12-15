namespace API.Application;

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

public class JwtAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public JwtAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        Microsoft.Extensions.Logging.ILoggerFactory logger,
        System.Text.Encodings.Web.UrlEncoder encoder,
        ISystemClock clock
    ) : base(options, logger, encoder, clock)
    { }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey("Authorization"))
            return AuthenticateResult.Fail("Missing Authorization Header");

        string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        if (string.IsNullOrEmpty(token))
            return AuthenticateResult.Fail("Invalid Token");

        try
        {
            var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY")!;
            var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER")!;
            var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE")!;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(jwtKey);

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = jwtIssuer,
                ValidateAudience = true,
                ValidAudience = jwtAudience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out var validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken!;
            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, jwtToken.Subject ?? "")
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
        catch
        {
            return AuthenticateResult.Fail("Invalid Token");
        }
    }
}
