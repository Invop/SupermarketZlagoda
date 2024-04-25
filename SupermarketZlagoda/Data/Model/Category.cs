using System.ComponentModel.DataAnnotations;

namespace SupermarketZlagoda.Data.Model;

public record Category
{
    public Guid Id { get; init; }
    [Required]
    [MinLength(3, ErrorMessage = "Name is too short!")]
    [MaxLength(50, ErrorMessage = "Name is too long (50 characters limit).")]
    public string Name { get; set; }
    public int CountStoreProducts { get; set; }
    public int CountPromoProducts { get; set; }
}