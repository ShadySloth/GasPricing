namespace API.Domain.Entities;

public class PasswordSalt
{
    public Guid Id { get; set; }

    public string Salt { get; set; } = null!;

    public Guid PasswordHashId { get; set; }
    public PasswordHash PasswordHash { get; set; } = null!;
}
