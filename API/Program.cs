using API.Application.Interfaces;
using API.Application.Services;
using API.Infrastructure;
using API.Infrastructure.Contexts;
using API.Infrastructure.Interfaces;
using API.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<IGasService, GasService>();
builder.Services.AddScoped<IGasTypeRepository, GasTypeRepository>();
builder.Services.AddTransient<IDbInitializer, DbInitializer>();
builder.Services.AddControllers();

builder.Services.AddDbContext<GasContext>(options =>
    options.UseInMemoryDatabase("GasDb"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var dbContext = services.GetService<GasContext>();
        var dbInitializer = services.GetService<IDbInitializer>();
        dbInitializer.Initialize(dbContext);
    }
}

app.UseAuthorization();
app.MapControllers();
await app.RunAsync();
