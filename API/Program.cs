using API.Application.Interfaces;
using API.Application.Services;
using API.Infrastructure;
using API.Infrastructure.Contexts;
using API.Infrastructure.Interfaces;
using API.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace API
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddScoped<IGasService, GasService>();
            builder.Services.AddScoped<IGasTypeRepository, GasTypeRepository>();
            builder.Services.AddTransient<IDbInitializer, DbInitializer>();
            builder.Services.AddControllers();

            if (builder.Environment.IsDevelopment())
            {
                builder.Services.AddDbContext<GasContext>(options =>
                    options.UseInMemoryDatabase("GasDb"));
            }
            else
            {
                string hostname = "db-postgres";
                string port = "5432";

                string username = Environment.GetEnvironmentVariable("POSTGRES_USER") ?? "";
                string password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD") ?? "";

                // Read the PostgreSQL username and password from Docker secret files
                var postgresUserFile = "/run/secrets/db_user";
                var postgresPasswordFile = "/run/secrets/db_password";

                if (File.Exists(postgresUserFile))
                {
                    username = File.ReadAllText(postgresUserFile).Trim();
                    Console.WriteLine($"username file found: {username}");
                }

                if (File.Exists(postgresPasswordFile))
                {
                    password = File.ReadAllText(postgresPasswordFile).Trim();
                }

                builder.Services.AddDbContext<GasContext>(options =>
                {
                    options.UseNpgsql(
                        $"Host={hostname};Port={port};Database=postgres;Username={username};Password={password};Trust Server Certificate=true;");
                });
            }

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();

                using var scope = app.Services.CreateScope();
                
                var services = scope.ServiceProvider;
                var dbContext = services.GetService<GasContext>();
                var dbInitializer = services.GetService<IDbInitializer>();
                dbInitializer!.InitializeDev(dbContext!);
            }
            else
            {
                // Apply migrations at application startup
                ApplyMigrations(app);
                 
                using var scope = app.Services.CreateScope();
                
                var services = scope.ServiceProvider;
                var dbContext = services.GetService<GasContext>();
                var dbInitializer = services.GetService<IDbInitializer>();
                dbInitializer!.InitializeProd(dbContext!);
            }

            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }

        private static void ApplyMigrations(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            
            var dbContext = scope.ServiceProvider.GetRequiredService<GasContext>();

            // Check and apply pending migrations
            var pendingMigrations = dbContext.Database.GetPendingMigrations();
            if (pendingMigrations.Any())
            {
                Console.WriteLine("Applying pending migrations...");
                dbContext.Database.Migrate();
                Console.WriteLine("Migrations applied successfully.");
            }
            else
            {
                Console.WriteLine("No pending migrations found.");
            }
        }
    }
}
