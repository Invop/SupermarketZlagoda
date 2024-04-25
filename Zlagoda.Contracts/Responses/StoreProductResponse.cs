namespace Zlagoda.Contracts.Responses;

public class StoreProductResponse
{
    public required string? Upc { get; init; }
    public string? UpcProm { get; init; }
    public required Guid ProductId { get; init; }
    public required decimal Price { get; init; }
    public required int Quantity { get; init; }
    public required bool IsPromotional { get; init; }

    public required int CheckCount { get; init; }
}