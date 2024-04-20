namespace SupermarketZlagoda.Data.Model;
using System.ComponentModel.DataAnnotations;


public record CustomerCard()
    {
    public Guid Id { get; init; }
    
    [Required(ErrorMessage = "Surname is required")]
    [MinLength(1, ErrorMessage = "Surname is too short!")]
    [StringLength(50, ErrorMessage = "Surname is too long (50 character limit).")]
    public string Surname { get; set; }
    
    [Required(ErrorMessage = "Name is required")]
    [MinLength(1, ErrorMessage = "Name is too short!")]
    [StringLength(50, ErrorMessage = "Name is too long (50 character limit).")]
    public string Name { get; set; }
    
    [StringLength(50, ErrorMessage = "Patronymic is too long (50 character limit).")]
    public string? Patronymic { get; set; }
    
    [Required(ErrorMessage = "Phone number is required")]
    [MinLength(9, ErrorMessage = "Phone number is too short!")]
    [MaxLength(13, ErrorMessage = "Phone number is too long (13 characters limit).")]
    [PhoneNumber(ErrorMessage = "Wrong format.")]
    public string Phone { get; set; }
    
    [StringLength(50, ErrorMessage = "City is too long (50 character limit).")]
    public string? City { get; set; }
    
    [StringLength(50, ErrorMessage = "Street is too long (50 character limit).")]
    public string? Street { get; set; }
    
    [StringLength(9, ErrorMessage = "Zip code is too long (50 character limit).")]
    public string? Index { get; set; }
    public int Percentage { get; set; }
    
}