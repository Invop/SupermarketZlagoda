namespace Zlagoda.Contracts.Responses;

public class SoldProductResponse
{
    public Guid ProductId { get; init; }
    public string ProductName { get; init; }
    public decimal SellingPrice { get; init; }
    public int TotalQuantity { get; init; }
}