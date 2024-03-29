namespace Zlagoda.Contracts.Requests;

public class CreateStoreProductRequest
{
    public required string Upc { get; init; }
    public string? UpcProm { get; init; }
    public required Guid ProductId { get; init; }
    public required decimal Price { get; init; }
    public required int Quantity { get; init; }
    public required bool IsPromotional { get; init; }
    
}