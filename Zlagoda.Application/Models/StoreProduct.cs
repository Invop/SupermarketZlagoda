namespace Zlagoda.Application.Models;

public class StoreProduct
{
    public required string? Upc { get; init; }
    public string? UpcProm { get; set; }
    public required Guid ProductId { get; set; }
    public required decimal Price { get; set; }
    public required int Quantity { get; set; }
    public required bool IsPromotional { get; set; }
    public int ChecksCount { get; set; }
}