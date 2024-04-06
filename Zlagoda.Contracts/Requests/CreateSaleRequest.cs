namespace Zlagoda.Contracts.Requests;

public class CreateSaleRequest
{
    public required string Upc { get; init; }
    public required Guid CheckNumber { get; init; }
    public required int ProductNumber { get; init; }
    public required decimal SellingPrice { get; init; }
}