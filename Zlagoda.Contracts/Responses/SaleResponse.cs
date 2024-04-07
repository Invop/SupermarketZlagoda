namespace Zlagoda.Contracts.Responses;

public class SaleResponse
{
    public required Guid CheckNumber { get; init; }
    public required string UPC { get; init; }
    public required int ProductNumber { get; init; }
    public required decimal SellingPrice { get; init; }
}