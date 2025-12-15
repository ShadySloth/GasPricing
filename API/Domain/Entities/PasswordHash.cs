namespace API.Domain.Entities;

public class PasswordHash
{
    public Guid Id { get; set; }

    public string Hash { get; set; } = null!;

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public PasswordSalt Salt { get; set; } = null!;
}
