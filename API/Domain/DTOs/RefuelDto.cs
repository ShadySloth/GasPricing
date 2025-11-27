namespace API.Domain.DTOs;

public class RefuelDto
{
    public int Liters {get; set;} = 0;
    public string GasTypeName {get; set; } = string.Empty;
    public bool Membership {get; set;} = false;
}