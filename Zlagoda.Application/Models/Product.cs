namespace Zlagoda.Application.Models;

public class Product
{
    public required Guid Id { get; init; }
    public required string Name { get; set; }
    public required int CategoryId { get; set; }
    public required int Characteristics { get; set; }
}