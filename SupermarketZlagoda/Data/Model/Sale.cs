using System.ComponentModel;

namespace SupermarketZlagoda.Data.Model;

public record Sale
{
    public Guid CheckNumber { get; set; }
    public string UPC { get; set; }
    public int ProductNumber { get; set; }
    public decimal SellingPrice { get; set; }
}