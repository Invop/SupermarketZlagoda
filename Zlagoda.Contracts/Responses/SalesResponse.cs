namespace Zlagoda.Contracts.Responses;

public class SalesResponse
{
    public required IEnumerable<SaleResponse> Items { get; init; } = Enumerable.Empty<SaleResponse>();
}