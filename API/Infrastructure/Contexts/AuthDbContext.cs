using API.Domain.Entities;

namespace API.Infrastructure.Contexts;

using Microsoft.EntityFrameworkCore;

public class AuthDbContext : DbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options)
        : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<PasswordHash> PasswordHashes => Set<PasswordHash>();
    public DbSet<PasswordSalt> PasswordSalts => Set<PasswordSalt>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasOne(u => u.PasswordHash)
            .WithOne(ph => ph.User)
            .HasForeignKey<PasswordHash>(ph => ph.UserId);

        modelBuilder.Entity<PasswordHash>()
            .HasOne(ph => ph.Salt)
            .WithOne(s => s.PasswordHash)
            .HasForeignKey<PasswordSalt>(s => s.PasswordHashId);
    }
}
