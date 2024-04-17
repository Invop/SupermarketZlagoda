namespace Zlagoda.Application.Models;

public class SoldProduct
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal SellingPrice { get; set; }
    public int TotalQuantity { get; set; }
}