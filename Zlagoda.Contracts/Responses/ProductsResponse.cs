namespace Zlagoda.Contracts.Responses;

public class ProductsResponse
{
    public required IEnumerable<ProductResponse> Items { get; init; } = Enumerable.Empty<ProductResponse>();
}