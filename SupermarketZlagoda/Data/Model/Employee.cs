using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SupermarketZlagoda.Data.Model;

public record Employee
{
    public Guid Id { get; init; }
    
    [Required]
    [MinLength(3, ErrorMessage = "Surname is too short!")]
    [StringLength(50, ErrorMessage = "Surname is too long (50 characters limit).")]
    public string Surname { get; set; }
    
    [Required]
    [MinLength(3, ErrorMessage = "Name is too short!")]
    [MaxLength(50, ErrorMessage = "Name is too long (50 characters limit).")]
    public string Name { get; set; }
    
    [MaxLength(50, ErrorMessage = "Patronymic is too long (50 characters limit).")]
    public string Patronymic { get; set; }
    
    [Required]
    public string Role { get; set; }

    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "Salary cannot be negative.")]
    public decimal Salary { get; set; }
    
    [Required]
    public DateOnly DateOfStart { get; set; }
    
    [Required]
    [MinimumAge(18, ErrorMessage = "Employee must be at least 18 years old.")]
    public DateOnly DateOfBirth { get; set; }
    
    [Required]
    [MaxLength(13, ErrorMessage = "Phone number is too long (13 characters limit).")]
    public string PhoneNumber { get; set; }
    
    [Required]
    [MaxLength(50, ErrorMessage = "City is too long (50 characters limit).")]
    public string City { get; set; }
    
    [Required]
    [MaxLength(50, ErrorMessage = "Street is too long (50 characters limit).")]
    public string Street { get; set; }
    
    [Required]
    [MaxLength(9, ErrorMessage = "ZipCode is too long (9 characters limit).")]
    public string ZipCode { get; set; }
}