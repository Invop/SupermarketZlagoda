using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

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

    [Required] public string Role { get; set; }

    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "Salary cannot be negative.")]
    public decimal Salary { get; set; }

    [Required] public DateTime? DateOfStart { get; set; }

    [Required]
    [MinimumAge(18, ErrorMessage = "Employee must be at least 18 years old.")]
    public DateTime? DateOfBirth { get; set; }

    [Required]
    [MinLength(9, ErrorMessage = "Phone number is too short!")]
    [MaxLength(13, ErrorMessage = "Phone number is too long (13 characters limit).")]
    [PhoneNumber(ErrorMessage = "Wrong format.")]
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

    [Required]
    [MinLength(3, ErrorMessage = "Login is too short!")]
    [MaxLength(20, ErrorMessage = "Login is too long (20 characters limit).")]
    public string UserLogin { get; set; }

    private string _hashPassword;

    public string UserPassword
    {
        get => _hashPassword;
        set => _hashPassword = UpdatePassword(value);
    }

    public static string UpdatePassword(string value)
    {
        var hashedBytes = SHA256.HashData(Encoding.UTF8.GetBytes(value));
        var builder = new StringBuilder();
        foreach (var t in hashedBytes)
            builder.Append(t.ToString("x2"));
        return builder.ToString();
    }
}