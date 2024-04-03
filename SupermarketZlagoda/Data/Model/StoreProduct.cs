using System.ComponentModel.DataAnnotations;

namespace SupermarketZlagoda.Data.Model;

public record StoreProduct
{
    public bool IsPromotional  { get; set; }  = false;
    
    [Required(ErrorMessage = "Upc is required")]
    [MinLength(1, ErrorMessage = "Upc is too short!")]
    [StringLength(12, ErrorMessage = "Upc too long (12 character limit).")]
    public string? Upc { get; set; } = null;
    public string? UpcProm { get; set; }
    [Required(ErrorMessage = "ProductId is required")]
    public Guid ProductId { get; set; }
    [Required(ErrorMessage = "Price is required")]
    public decimal Price { get; set; } = decimal.Zero;
    public int Quantity { get; set; } = 0;
    
    
}