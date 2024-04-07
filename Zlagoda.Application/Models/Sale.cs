namespace Zlagoda.Application.Models;

public class Sale
{
    public required string Upc { get; set; } 
    public required Guid CheckNumber { get; set; }
    public required int ProductNumber { get; set; }
    public required decimal SellingPrice { get; set; }
}