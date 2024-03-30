using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.JavaScript;
using SupermarketZlagoda.Components.Pages;

namespace SupermarketZlagoda.Data.Model;

public record Product
{
    public Guid Id { get; init; }

    [Required(ErrorMessage = "A category is required")]
    public Guid CategoryId { get; set; }

    [Required(ErrorMessage = "name is required")]
    [MinLength(3, ErrorMessage = "Name is too short!")]
    [StringLength(50, ErrorMessage = "Name too long (50 character limit).")]
    public string Name { get; set; } = string.Empty;

    public string Characteristics { get; set; } = string.Empty;
}