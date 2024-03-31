namespace SupermarketZlagoda.Data.Model;

public record StoreProduct
{
    private bool _isPromotional = false;
    public string Upc { get; set; } = string.Empty;
    public string? UpcProm { get; set; } = string.Empty;
    public Guid ProductId { get; set; }
    public decimal Price { get; set; } = decimal.Zero;
    public int Quantity { get; set; } = 0;

    public bool IsPromotional
    {
        get => _isPromotional;
        set {
            if (value is false)
            {
                UpcProm = string.Empty;
            }
            _isPromotional = value;
        }
    }
}