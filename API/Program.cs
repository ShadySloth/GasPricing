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


// Tilføj alle nødvendige envs til Configuration
builder.Configuration["Jwt:Key"] = Environment.GetEnvironmentVariable("JWT_KEY") 
                                   ?? throw new Exception("JWT_KEY missing");
builder.Configuration["Jwt:Issuer"] = Environment.GetEnvironmentVariable("JWT_ISSUER") 
                                      ?? throw new Exception("JWT_ISSUER missing");
builder.Configuration["Jwt:Audience"] = Environment.GetEnvironmentVariable("JWT_AUDIENCE") 
                                        ?? throw new Exception("JWT_AUDIENCE missing");
builder.Configuration["Jwt:ExpiresMinutes"] = Environment.GetEnvironmentVariable("JWT_EXPIRES_MINUTES") 
                                              ?? "60";
builder.Configuration["Password:Pepper"] = Environment.GetEnvironmentVariable("PASSWORD_PEPPER") 
                                           ?? throw new Exception("PASSWORD_PEPPER missing");

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
