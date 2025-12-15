namespace API.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = null!;

    public PasswordHash PasswordHash { get; set; } = null!;
}
