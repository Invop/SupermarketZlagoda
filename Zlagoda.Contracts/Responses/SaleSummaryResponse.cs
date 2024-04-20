namespace Zlagoda.Contracts.Responses;

public class SaleSummaryResponse
{
    public string? ProductName { get; init; }
    public decimal? SellingPrice { get; init; }
    public int? TotalQuantity { get; init; }
}