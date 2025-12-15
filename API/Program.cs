using System.Text;
using API.Application;
using API.Application.Interfaces;
using API.Application.Services;
using API.Infrastructure;
using API.Infrastructure.Contexts;
using API.Infrastructure.Interfaces;
using API.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

var envPath = Path.Combine(Directory.GetCurrentDirectory(), "..", ".env");
Env.Load(envPath);

// Tilføj pepper og JWT config til builder.Configuration
Environment.SetEnvironmentVariable("JWT_KEY", "TEST_SUPER_SECRET_KEY");
Environment.SetEnvironmentVariable("JWT_ISSUER", "TestIssuer");
Environment.SetEnvironmentVariable("JWT_AUDIENCE", "TestAudience");
Environment.SetEnvironmentVariable("JWT_EXPIRES_MINUTES", "60");
Environment.SetEnvironmentVariable("PASSWORD_PEPPER", "TEST_PEPPER");


// EF Core for Auth
builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseInMemoryDatabase("auth")
);

// DI
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Authentication & Authorization
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Bearer";
    options.DefaultChallengeScheme = "Bearer";
})
.AddScheme<Microsoft.AspNetCore.Authentication.AuthenticationSchemeOptions, JwtAuthenticationHandler>(
    "Bearer", options => { }
);

builder.Services.AddAuthorization(); // kun endpoints med [Authorize]

// Controllers, Swagger, other services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Optional: other services
builder.Services.AddScoped<IGasService, GasService>();
builder.Services.AddScoped<IGasTypeRepository, GasTypeRepository>();
builder.Services.AddTransient<IDbInitializer, DbInitializer>();
builder.Services.AddDbContext<GasContext>(options =>
    options.UseInMemoryDatabase("GasDb")
);

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var dbContext = services.GetService<GasContext>();
        var dbInitializer = services.GetService<IDbInitializer>();
        dbInitializer!.Initialize(dbContext!);
    }
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
