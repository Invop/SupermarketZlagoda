namespace Zlagoda.Application.Models;

public class StoreProduct
{
    public required Guid Id { get; init; }
    public required string Name { get; set; }
    public required int CategoryId { get; set; }
    public required string Characteristics { get; set; }
}