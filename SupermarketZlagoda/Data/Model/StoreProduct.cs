namespace SupermarketZlagoda.Data.Model;

public record StoreProduct
{
    public string Upc { get; set; } = string.Empty;
    public string? UpcProm { get; set; } = string.Empty;
    public Guid ProductId { get; set; }
    public decimal Price { get; set; } = decimal.Zero;
    public int Quantity { get; set; } = 0;
    public bool IsPromotional { get; set; } = false;
}