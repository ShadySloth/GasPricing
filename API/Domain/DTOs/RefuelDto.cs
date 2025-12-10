namespace API.Domain.DTOs;

public class RefuelDto
{
    public required int Liters { get; set; }
    public required string GasTypeName { get; set; }
    public required bool Membership { get; set; }
}