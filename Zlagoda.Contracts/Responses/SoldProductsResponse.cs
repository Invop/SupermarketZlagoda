namespace Zlagoda.Contracts.Responses;

public class SoldProductsResponse
{
    public required IEnumerable<SoldProductResponse> Items { get; init; } = Enumerable.Empty<SoldProductResponse>();
}