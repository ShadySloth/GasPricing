namespace API.Domain.Entities;

public class GasType
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Price { get; set; }
}