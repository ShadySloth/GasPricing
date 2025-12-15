namespace API.Domain.DTOs;

public class RefuelDto
{
    /**
     * The number of liters to refuel.
     */
    public required int Liters { get; set; }
    /**
     * The type of gas to refuel.
     */
    public required string GasTypeName { get; set; }
    /**
     * Membership status for start discount.
     */
    public required bool Membership { get; set; }
}