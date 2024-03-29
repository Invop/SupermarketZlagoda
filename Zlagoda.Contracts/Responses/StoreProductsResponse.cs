namespace Zlagoda.Contracts.Responses;

public class StoreProductsResponse
{
    public required IEnumerable<StoreProductResponse> Items { get; init; } = Enumerable.Empty<StoreProductResponse>();
}