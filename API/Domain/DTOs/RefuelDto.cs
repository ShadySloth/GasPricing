namespace API.Domain.DTOs;

public class RefuelDto
{
    /// <example>50</example>
    public required int Liters { get; set; }

    /// <example>Regular</example>
    public required string GasTypeName { get; set; }
    
    /// <example>true</example>
    public required bool Membership { get; set; }
}