namespace Zlagoda.Contracts.Responses;

public class SalesSummaryResponse
{
    public required IEnumerable<SaleSummaryResponse> Items { get; init; } = Enumerable.Empty<SaleSummaryResponse>();
}