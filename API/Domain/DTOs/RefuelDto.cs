namespace API.Domain.DTOs;

public class RefuelDto
{
    public int Liters { get; set; }
    public required string GasTypeName { get; set; }
    public bool Membership { get; set; }
}